using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.OpenAQ.Integrations
{
    internal class GetSensorsByLocationIdHandler : ICommandHandler<GetSensorsByLocationIdCommand, List<SensorDTO>>
    {
        private readonly IOpenAQService _openAQService;
        public GetSensorsByLocationIdHandler(IOpenAQService openAQService)
        {
            _openAQService = openAQService;
        }

        public async Task<List<SensorDTO>> ExecuteAsync(GetSensorsByLocationIdCommand command, CancellationToken ct)
        {
            var response = await _openAQService.GetMeasurementsByLocationIdAsync(command.locationId);
            if (response is null || response.Results is null || response.Results.Count == 0)
            {
                return [];
            }
            return response.Results;
        }
    }
}
