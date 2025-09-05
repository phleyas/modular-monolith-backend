using AirQuality.OpenAQ.Contracts;

namespace AirQuality.Locations.Endpoints
{
    public class LatestMeasurementsResponse
    {
        public List<MeasurementDTO> Measurements { get; set; }
    }
}
