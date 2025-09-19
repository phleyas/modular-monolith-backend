using AirQuality.OpenAQ.Contracts;

namespace AirQuality.Api.Endpoints
{
    public class SensorsResponse
    {
        public List<SensorDTO> Sensors { get; set; }
    }
}
