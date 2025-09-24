using AirQuality.OpenAQ.Contracts;

namespace AirQuality.OpenAQ.Data
{
    internal interface ILocationsRepository
    {
        Task<LocationDTO?> GetLocationByIdAsync(int id);
        Task<List<LocationDTO>> GetAllLocationsAsync();
        Task UpdateLocationsAsync(List<LocationDTO> locations);
        Task UpdateLocationAsync(LocationDTO location);
        Task<List<LocationDTO>> GetLocationsByCoordinatesAsync(double latitude, double longitude, double radiusMeters, int? limit);
        Task RemoveLocationAsync(int id);
        Task SaveChangesAsync();
    }
}