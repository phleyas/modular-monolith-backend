using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.OpenAQ.Integrations
{
    internal class GetLocationsByCoordinatesHandler : ICommandHandler<GetLocationsByCoordinatesCommand, List<LocationDTO>>
    {
        private readonly IOpenAQService _openAQService;
        public GetLocationsByCoordinatesHandler(IOpenAQService openAQService)
        {
            _openAQService = openAQService;
        }
        public async Task<List<LocationDTO>> ExecuteAsync(GetLocationsByCoordinatesCommand command, CancellationToken ct)
        {
            var result = await _openAQService.GetLocationsByCoordinatesAsync(command.latitude, command.longuitude, command.radius, command.limit);
            if (result is null || result.Results is null || result.Results.Count == 0)
            {
                return [];
            }
            return result.Results;
        }
    }
}
