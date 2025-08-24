using AirQuality.OpenAQ.Contracts;

namespace AirQuality.OpenAQ
{
    internal interface ISensorLocationsService
    {
        public Task<OpenAQResponse<SensorLocationDTO>> GetLocationsByCoordinatesAsync(double? latitude, double? longuitude, int? radius, int? limit);

        public Task<OpenAQResponse<SensorLocationDTO>> GetLocationsByIdAsync(int id);
    }
}
