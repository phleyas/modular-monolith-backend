using AirQuality.Geolocation.Contracts;
using FastEndpoints;

namespace AirQuality.Geocoding
{
    internal class GetCoordinatesByLocationHandler : ICommandHandler<GetCoordinatesByLocationCommand, GeoLocation>
    {
        private readonly IGeocodingService _geocoding;
        public GetCoordinatesByLocationHandler(IGeocodingService geocoding)
        {
            _geocoding = geocoding;
        }
        public async Task<GeoLocation> ExecuteAsync(GetCoordinatesByLocationCommand command, CancellationToken ct)
        {
            return await _geocoding.GetCoordinatesByLocationAsync(command.Country, command.City);
        }
    }
}
