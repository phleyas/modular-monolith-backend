using AirQuality.Geocoding.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.Geocoding.Data
{
    internal class GeoCodingInfoRepository : IGeoCodingInfoRepository
    {
        private readonly GeoCodingDBContext _context;

        public GeoCodingInfoRepository(GeoCodingDBContext context)
        {
            _context = context;
        }

        public async Task<GeoCodingInfo> GetByIdAsync(int id)
        {
            return await _context.geoCodingInfos.FirstOrDefaultAsync(g => g.Id == id);
        }

        // Get all GeoCodingInfo records
        public async Task<List<GeoCodingInfo>> GetAllAsync()
        {
            return await _context.geoCodingInfos.ToListAsync();
        }

        // Update an existing GeoCodingInfo
        public async Task UpdateAsync(GeoCodingInfo geoCodingInfo)
        {
            _context.geoCodingInfos.Update(geoCodingInfo);
            await _context.SaveChangesAsync();
        }

        // Delete a GeoCodingInfo by its ID
        public async Task DeleteAsync(int id)
        {
            var geoCodingInfo = await _context.geoCodingInfos.FirstOrDefaultAsync(g => g.Id == id);
            if (geoCodingInfo != null)
            {
                _context.geoCodingInfos.Remove(geoCodingInfo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GeoCodingInfo> GetByCityAndCountryAsync(string city, string country)
        {
            var geoCodingInfo = await _context.geoCodingInfos
                .Include(g => g.GeoLocation)
                .FirstOrDefaultAsync(g => g.City == city && g.Country == country);


            if (geoCodingInfo is null)
            {
                return null;
            }

            if (geoCodingInfo.LastUpdate <= DateTime.UtcNow.AddDays(-7))
            {
                await this.DeleteAsync(geoCodingInfo.Id);

                return null;
            }

            return geoCodingInfo;

        }

        public async Task AddGeoCodingInfoWithLocationAsync(GeoCodingInfo geoCodingInfo)
        {
            // Check if the GeoLocation already exists in the database
            var existingGeoLocation = await _context.geoLocations
                .FirstOrDefaultAsync(gl => gl.Lat == geoCodingInfo.GeoLocation.Lat && gl.Lon == geoCodingInfo.GeoLocation.Lon);

            if (existingGeoLocation != null)
            {
                // Use the existing GeoLocation
                geoCodingInfo.GeoLocationId = existingGeoLocation.Id.Value;
            }
            else
            {
                // Add the new GeoLocation to the database
                var newGeoLocation = geoCodingInfo.GeoLocation;
                await _context.geoLocations.AddAsync(newGeoLocation);
                await _context.SaveChangesAsync(); // Ensure the GeoLocation is saved and its Id is generated

                // Use the newly created GeoLocation's Id
                geoCodingInfo.GeoLocationId = newGeoLocation.Id.Value;
            }

            // Detach the GeoLocation to avoid EF trying to insert it again
            geoCodingInfo.GeoLocation = null;

            // Add the GeoCodingInfo to the database
            await _context.geoCodingInfos.AddAsync(geoCodingInfo);
            await _context.SaveChangesAsync();
        }
    }
}
