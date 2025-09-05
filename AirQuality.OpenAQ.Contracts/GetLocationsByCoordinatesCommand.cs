using FastEndpoints;

namespace AirQuality.OpenAQ.Contracts
{
    public class GetLocationsByCoordinatesCommand : ICommand<List<LocationDTO>>
    {
        public double? latitude;
        public double? longuitude;
        public int? radius;
        public int? limit;
    }
}
