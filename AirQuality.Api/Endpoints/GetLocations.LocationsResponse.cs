using AirQuality.OpenAQ.Contracts;

namespace AirQuality.Api.Endpoints
{
    public class LocationsResponse
    {
        public List<LocationDTO> Locations { get; set; }
    }
}
