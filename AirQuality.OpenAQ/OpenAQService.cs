using AirQuality.OpenAQ.Contracts;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace AirQuality.OpenAQ
{
    internal class OpenAQService : IOpenAQService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public OpenAQService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<OpenAQResponse<LocationDTO>> GetLocationsByCoordinatesAsync(double? latitude = 50.962838000069745, double? longuitude = 7.004604999927973, int? radius = 10000, int? limit = 10)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            var response = await _httpClient.GetAsync(
                $"locations?limit={limit}&coordinates={latitude?.ToString(nfi)},{longuitude?.ToString(nfi)}&radius={radius}"
            );
            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var result = JsonSerializer.Deserialize<OpenAQResponse<LocationDTO>>(json, _jsonOptions);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error deserializing response: {e.Message}");
                return null;
            }
        }

        public async Task<OpenAQResponse<LocationDTO>> GetLocationsByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"locations/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenAQResponse<LocationDTO>>(json, _jsonOptions);

            return result;
        }

        public async Task<OpenAQResponse<SensorDTO>> GetSensorsByLocationIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(
             $"locations/{id}/sensors"
         );
            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var result = JsonSerializer.Deserialize<OpenAQResponse<SensorDTO>>(json, _jsonOptions);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error deserializing response: {e.Message}");
                return null;
            }
        }
        public async Task<OpenAQResponse<ParameterDTO>> GetParametersAsync(int? id)
        {
            var response = await _httpClient.GetAsync(
               id.HasValue ? $"parameters/{id}" : "parameters"
            );
            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var result = JsonSerializer.Deserialize<OpenAQResponse<ParameterDTO>>(json, _jsonOptions);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error deserializing response: {e.Message}");
                return null;
            }
        }
    }
}
