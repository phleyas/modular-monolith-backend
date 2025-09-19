using AirQuality.Geocoding.Contracts;
using AirQuality.Geocoding.Data;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace AirQuality.Geocoding.Integration
{
    internal class UpdateGeoCodingInfoEventHandler : IEventHandler<UpdateGeoCodingInfoEvent>
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateGeoCodingInfoEventHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(UpdateGeoCodingInfoEvent eventModel, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var _repository = scope.Resolve<IGeoCodingInfoRepository>();
            // Retrieve the GeoCodingInfo from the database
            var geoCodingInfo = await _repository.GetByCityAndCountryAsync(eventModel.City, eventModel.Country);

            if (geoCodingInfo != null || eventModel.GeoLocation is null)
            {
                return;
            }

            // Create a new GeoCodingInfo if it doesn't exist
            var newGeoCodingInfo = new GeoCodingInfo
            {
                LastUpdate = DateTime.UtcNow,
                City = eventModel.City,
                Country = eventModel.Country,
                GeoLocation = eventModel.GeoLocation
            };

            await _repository.AddGeoCodingInfoWithLocationAsync(newGeoCodingInfo);
        }
    }
}
