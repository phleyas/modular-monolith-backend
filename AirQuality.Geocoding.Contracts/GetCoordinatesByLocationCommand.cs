using FastEndpoints;

namespace AirQuality.Geocoding.Contracts
{
    public class GetCoordinatesByLocationCommand : ICommand<GeoLocation>
    {
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}
