using AirQuality.OpenAQ.Contracts;

namespace AirQuality.Locations.Endpoints
{
    public class LocationsResponse
    {
        public List<SensorLocationDTO> Locations { get; set; }
    }
}
