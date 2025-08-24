using AirQuality.Geolocation.Contracts;
namespace AirQuality.Geocoding
{
    internal interface IGeocodingService
    {
        public Task<GeoLocation> GetCoordinatesByLocationAsync(string country, string city);
    }
}
