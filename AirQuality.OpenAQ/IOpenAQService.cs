using AirQuality.OpenAQ.Contracts;

namespace AirQuality.OpenAQ
{
    internal interface IOpenAQService
    {
        public Task<OpenAQResponse<ParameterDTO>> GetParametersAsync(int? id = null);
        public Task<OpenAQResponse<SensorDTO>> GetSensorsByLocationIdAsync(int id);
        public Task<OpenAQResponse<LocationDTO>> GetLocationsByCoordinatesAsync(double? latitude, double? longuitude, int? radius, int? limit);
        public Task<OpenAQResponse<LocationDTO>> GetLocationsByIdAsync(int id);
    }
}
