using AirQuality.OpenAQ.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.OpenAQ.Data
{
    internal class SensorRepository : ISensorRepository
    {
        private readonly OpenAQDbContext _context;
        public SensorRepository(OpenAQDbContext context) => _context = context;

        public async Task<List<SensorDTO>> GetAllSensorsAsync()
        {
            return await _context.Sensors
                 .Include(s => s.Parameter)
                 .Include(s => s.Latest)
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<SensorDTO?> GetSensorByIdAsync(int id)
        {
            return await _context.Sensors
                .Include(s => s.Parameter)
                .Include(s => s.Latest)
                .Where(s => s.LocationId.HasValue)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SensorDTO>> GetSensorsByLocationIdAsync(int locationId)
        {
            return await _context.Sensors
                .Include(s => s.Parameter)
                .Include(s => s.Latest)
                .AsNoTracking()
                .Where(s => s.LocationId == locationId)
                .Where(s => s.LastUpdate > DateTime.UtcNow.AddHours(-1))
                .ToListAsync();
        }

        public async Task RemoveSensorAsync(int id)
        {
            var existing = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == id);
            if (existing is null) return;
            _context.Sensors.Remove(existing);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public async Task UpdateSensorAsync(SensorDTO sensor)
        {
            if (sensor.Parameter is not null)
                sensor.Parameter = await ResolveParameterAsync(sensor.Parameter);

            sensor.Latest ??= new LatestDTO { HasValueMarker = true };

            var existing = await _context.Sensors
                .Include(s => s.Latest)
                .FirstOrDefaultAsync(s => s.Id == sensor.Id);

            if (existing is not null)
            {
                existing.Name = sensor.Name;
                existing.Parameter = sensor.Parameter;
                existing.DatetimeFirst = sensor.DatetimeFirst;
                existing.DatetimeLast = sensor.DatetimeLast;
                existing.Coverage = sensor.Coverage;
                existing.Latest = sensor.Latest;
                existing.Summary = sensor.Summary;
                existing.LastUpdate = sensor.LastUpdate;
                existing.LocationId = sensor.LocationId;
            }
            else
            {
                await _context.Sensors.AddAsync(sensor);
            }
        }

        public async Task UpdateSensorsAsync(List<SensorDTO> sensors)
        {
            if (sensors is null || sensors.Count == 0) return;

            foreach (var sensor in sensors)
            {
                if (sensor is null) continue;

                await UpdateSensorAsync(sensor);
            }
        }

        private async Task<ParameterDTO> ResolveParameterAsync(ParameterDTO incoming)
        {
            var id = incoming.Id;
            var tracked = _context.ChangeTracker.Entries<ParameterDTO>()
                .FirstOrDefault(e => e.Entity.Id == id)?.Entity;
            if (tracked is not null) return tracked;

            var local = _context.Parameters.Local.FirstOrDefault(p => p.Id == id);
            if (local is not null) return local;

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
    }
}
