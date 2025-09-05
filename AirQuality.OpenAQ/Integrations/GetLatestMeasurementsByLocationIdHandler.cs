using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.OpenAQ.Integrations
{
    internal class GetLatestMeasurementsByLocationIdHandler : ICommandHandler<GetLatestMeassurementsByLocationIdCommand, List<MeasurementDTO>>
    {
        private readonly IOpenAQService _locationsService;
        public GetLatestMeasurementsByLocationIdHandler(IOpenAQService locationsService)
        {
            _locationsService = locationsService;
        }

        public async Task<List<MeasurementDTO>> ExecuteAsync(GetLatestMeassurementsByLocationIdCommand command, CancellationToken ct)
        {
            var response = await _locationsService.GetMeasurementsByLocationIdAsync(command.locationId);
            if (response is null || response.Results is null || response.Results.Count == 0)
            {
                return [];
            }
            return response.Results;
        }
    }
}
