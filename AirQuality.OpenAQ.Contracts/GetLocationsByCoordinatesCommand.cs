using FastEndpoints;

namespace AirQuality.OpenAQ.Contracts
{
    public class GetLocationsByCoordinatesCommand : ICommand<OpenAQResponse<SensorLocationDTO>>
    {
        public double? latitude;
        public double? longuitude;
        public int? radius;
        public int? limit;
    }
}
