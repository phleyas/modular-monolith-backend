using AirQuality.OpenAQ.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.OpenAQ.Data
{
    internal class LocationsRepository : ILocationsRepository
    {
        private readonly OpenAQDbContext _context;

        public LocationsRepository(OpenAQDbContext context)
        {
            _context = context;
        }

        public async Task<List<LocationDTO>> GetAllLocationsAsync()
        {
            return await _context
                .Locations
                .Include(l => l.Sensors)
                .ThenInclude(s => s.Parameter)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LocationDTO?> GetLocationByIdAsync(int id)
        {
            return await _context.Locations
                .Include(l => l.Sensors)
                .ThenInclude(s => s.Parameter)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<LocationDTO>> GetLocationsByCoordinatesAsync(double latitude, double longitude, double radiusMeters, int? limit)
        {
            // Convert meters to kilometers for HaversineKm comparison
            var radiusKm = radiusMeters / 1000.0;

            var candidates = await _context.Locations
                .Include(l => l.Sensors)
                .ThenInclude(s => s.Parameter)
                .AsNoTracking()
                .Where(l => l.Coordinates != null)
                .Where(l => l.LastUpdate > DateTime.UtcNow.AddHours(-1))
                .ToListAsync();

            var filtered = candidates
                .Select(l => new
                {
                    Location = l,
                    Lat = l.Coordinates?.Latitude,
                    Lon = l.Coordinates?.Longitude
                })
                .Where(x => x.Lat.HasValue && x.Lon.HasValue)
                .Select(x => new
                {
                    x.Location,
                    DistanceKm = HaversineKm(latitude, longitude, x.Lat!.Value, x.Lon!.Value)
                })
                .Where(x => x.DistanceKm <= radiusKm)
                .OrderBy(x => x.DistanceKm)
                .AsEnumerable();

            if (limit is > 0)
                filtered = filtered.Take(limit.Value);

            return filtered
                .Select(x => x.Location)
                .ToList();
        }

        public async Task UpdateLocationsAsync(List<LocationDTO> locations)
        {
            foreach (var location in locations)
            {
                await UpdateLocationAsync(location);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371.0;
            static double ToRad(double deg) => Math.PI * deg / 180.0;

            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public async Task RemoveLocationAsync(int id)
        {
            var existing = await _context.Locations
                .Include(l => l.Sensors)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (existing is null)
                return;

            if (existing.Sensors is { Count: > 0 })
                _context.RemoveRange(existing.Sensors);

            _context.Locations.Remove(existing);
        }

        private void NormalizeCountryDates(LocationDTO location)
        {
            if (location.Country is null)
                return;

            // Prefer UTC; fall back to Local if UTC is missing
            var locFirst = location.DatetimeFirst?.Utc ?? location.DatetimeFirst?.Local;
            var locLast = location.DatetimeLast?.Utc ?? location.DatetimeLast?.Local;

            if (!location.Country.DatetimeFirst.HasValue && locFirst.HasValue)
                location.Country.DatetimeFirst = locFirst.Value;

            if (!location.Country.DatetimeLast.HasValue && locLast.HasValue)
                location.Country.DatetimeLast = locLast.Value;
        }

        // Ensure that sensors reference a single tracked ParameterDTO instance and that it exists in DB
        private async Task NormalizeSharedReferencesAsync(LocationDTO location)
        {
            if (location.Sensors is null)
                return;

            foreach (var sensor in location.Sensors)
            {
                if (sensor?.Parameter is null)
                    continue;

                var resolved = await ResolveParameterAsync(sensor.Parameter);
                sensor.Parameter = resolved;
            }
        }

        private async Task<ParameterDTO> ResolveParameterAsync(ParameterDTO incoming)
        {
            var id = incoming.Id;

            var tracked = _context.ChangeTracker.Entries<ParameterDTO>()
                                  .FirstOrDefault(e => e.Entity.Id == id)
                                  ?.Entity;
            if (tracked is not null)
                return tracked;

            var local = _context.Parameters.Local.FirstOrDefault(p => p.Id == id);
            if (local is not null)
                return local;

            var fromDb = await _context.Parameters.FindAsync(id);
            if (fromDb is not null)
            {
                fromDb.Name = incoming.Name;
                fromDb.Units = incoming.Units;
                fromDb.DisplayName = incoming.DisplayName;
                fromDb.Description = incoming.Description;
                return fromDb;
            }

            var newParam = new ParameterDTO
            {
                Id = incoming.Id,
                Name = incoming.Name,
                Units = incoming.Units,
                DisplayName = incoming.DisplayName,
                Description = incoming.Description
            };

            await _context.Parameters.AddAsync(newParam);
            return newParam;
        }

        public async Task UpdateLocationAsync(LocationDTO location)
        {
            // Fill missing country dates from the location dates (fallback)
            NormalizeCountryDates(location);

            // Ensure related Parameters exist and are tracked uniquely
            await NormalizeSharedReferencesAsync(location);

            var existing = await _context.Locations
                .Include(l => l.Sensors) // load sensors so EF knows existing ones
                .ThenInclude(s => s.Latest) // include owned latest to avoid duplicate inserts
                .FirstOrDefaultAsync(l => l.Id == location.Id);

            if (existing is not null)
            {
                existing.Name = location.Name;
                existing.Locality = location.Locality;
                existing.Timezone = location.Timezone;
                existing.Country = location.Country;
                existing.Owner = location.Owner;
                existing.Provider = location.Provider;
                existing.IsMobile = location.IsMobile;
                existing.IsMonitor = location.IsMonitor;
                existing.Instruments = location.Instruments;
                // existing.Sensors will be updated individually below
                existing.Coordinates = location.Coordinates;
                existing.Licenses = location.Licenses;
                existing.Bounds = location.Bounds;
                existing.Distance = location.Distance;
                existing.DatetimeFirst = location.DatetimeFirst;
                existing.DatetimeLast = location.DatetimeLast;
                existing.LastUpdate = location.LastUpdate;

                if (location.Sensors is not null)
                {
                    foreach (var incomingSensor in location.Sensors)
                    {
                        if (incomingSensor is null) continue;

                        // Resolve parameter reference again if necessary
                        if (incomingSensor.Parameter is not null)
                            incomingSensor.Parameter = await ResolveParameterAsync(incomingSensor.Parameter);

                        incomingSensor.Latest ??= new LatestDTO { HasValueMarker = true };

                        var existingSensor = existing.Sensors?.FirstOrDefault(s => s.Id == incomingSensor.Id);
                        if (existingSensor is not null)
                        {
                            existingSensor.Name = incomingSensor.Name;
                            existingSensor.Parameter = incomingSensor.Parameter;
                            existingSensor.DatetimeFirst = incomingSensor.DatetimeFirst;
                            existingSensor.DatetimeLast = incomingSensor.DatetimeLast;
                            existingSensor.Coverage = incomingSensor.Coverage;
                            existingSensor.Latest = incomingSensor.Latest;
                            existingSensor.Summary = incomingSensor.Summary;
                            existingSensor.LastUpdate = incomingSensor.LastUpdate;
                            existingSensor.LocationId = existing.Id;
                        }
                        else
                        {
                            incomingSensor.LocationId = existing.Id;
                            await _context.Sensors.AddAsync(incomingSensor);
                        }
                    }
                }
            }
            else
            {
                // New location
                if (location.Sensors is not null)
                {
                    foreach (var sensor in location.Sensors)
                    {
                        if (sensor is null) continue;
                        if (sensor.Parameter is not null)
                            sensor.Parameter = await ResolveParameterAsync(sensor.Parameter);
                        sensor.Latest ??= new LatestDTO { HasValueMarker = true };
                        sensor.LocationId = location.Id; // explicit since PK not generated
                    }
                }
                await _context.Locations.AddAsync(location);
            }
        }
    }
}