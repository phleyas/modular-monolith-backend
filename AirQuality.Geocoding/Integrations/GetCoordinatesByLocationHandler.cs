using AirQuality.Geocoding.Contracts;
using AirQuality.Geocoding.Data;
using FastEndpoints;

namespace AirQuality.Geocoding.Integration
{
    internal class GetCoordinatesByLocationHandler : ICommandHandler<GetCoordinatesByLocationCommand, GeoLocation>
    {
        private readonly IGeoCodingInfoRepository _repository;
        private readonly IGeocodingService _geocoding;

        public GetCoordinatesByLocationHandler(IGeoCodingInfoRepository repository, IGeocodingService geocoding)
        {
            _repository = repository;
            _geocoding = geocoding;
        }

        public async Task<GeoLocation> ExecuteAsync(GetCoordinatesByLocationCommand command, CancellationToken ct)
        {
            var geoCodingInfo = await _repository.GetByCityAndCountryAsync(command.City, command.Country);
            if (geoCodingInfo != null)
            {
                // Return the GeoLocation from the database
                return geoCodingInfo.GeoLocation;
            }
            return await _geocoding.GetCoordinatesByLocationAsync(command.City, command.Country);
        }
    }
}
