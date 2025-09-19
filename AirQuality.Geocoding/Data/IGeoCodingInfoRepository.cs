using AirQuality.Geocoding.Contracts;

namespace AirQuality.Geocoding.Data
{
    public interface IGeoCodingInfoRepository
    {
        Task<GeoCodingInfo> GetByIdAsync(int id);
        Task<List<GeoCodingInfo>> GetAllAsync();
        Task UpdateAsync(GeoCodingInfo geoCodingInfo);
        Task DeleteAsync(int id);
        Task<GeoCodingInfo> GetByCityAndCountryAsync(string city, string country);
        Task AddGeoCodingInfoWithLocationAsync(GeoCodingInfo geoCodingInfo);
    }
}
