using AirQuality.Geocoding.Contracts;
using System.Diagnostics;
using System.Text.Json;

namespace AirQuality.Geocoding
{
    internal class NominatimGeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public NominatimGeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<GeoLocation> GetCoordinatesByLocationAsync(string country = "germany", string city = "cologne")
        {
            var response = await _httpClient.GetAsync(
                $"search?q={country},{city}&format=json"
            );
            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var results = JsonSerializer.Deserialize<List<NominatimResponse>>(json, _jsonOptions);
                if (results == null)
                {
                    throw new Exception("Could not deserialize NominatimResponse");
                }
                var geoLocations = results.Select(r => new GeoLocation
                {
                    Lat = r.Lat,
                    Lon = r.Lon,
                    Name = r.Name,
                    DisplayName = r.DisplayName,
                    Boundingbox = r.Boundingbox
                }).ToList();

                return geoLocations.FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error deserializing response: {e.Message}");
                return null;
            }
        }
    }
}
