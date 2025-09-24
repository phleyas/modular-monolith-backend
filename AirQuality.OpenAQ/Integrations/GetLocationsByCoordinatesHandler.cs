using AirQuality.OpenAQ.Contracts;
using AirQuality.OpenAQ.Data;
using FastEndpoints;

namespace AirQuality.OpenAQ.Integrations
{
    internal class GetLocationsByCoordinatesHandler : ICommandHandler<GetLocationsByCoordinatesCommand, List<LocationDTO>>
    {
        private readonly IOpenAQService _openAQService;
        private readonly ILocationsRepository _locationsRepository;

        public GetLocationsByCoordinatesHandler(IOpenAQService openAQService, ILocationsRepository locationsRepository)
        {
            _openAQService = openAQService;
            _locationsRepository = locationsRepository;
        }
        public async Task<List<LocationDTO>> ExecuteAsync(GetLocationsByCoordinatesCommand command, CancellationToken ct)
        {
            if (!command.latitude.HasValue || !command.longuitude.HasValue)
                return [];

            var lat = command.latitude.Value;
            var lon = command.longuitude.Value;

            // radius in meters 
            var radiusMeters = command.radius ?? 10000;

            var cached = await _locationsRepository.GetLocationsByCoordinatesAsync(lat, lon, radiusMeters, command.limit);
            if (cached.Count > 0)
                return cached;

            var api = await _openAQService.GetLocationsByCoordinatesAsync(lat, lon, radiusMeters, command.limit);
            if (api is null || api.Results is null || api.Results.Count == 0)
                return [];
            foreach (var location in api.Results)
            {
                location.LastUpdate = DateTime.UtcNow;
            }
            return api.Results;
        }
    }
}
