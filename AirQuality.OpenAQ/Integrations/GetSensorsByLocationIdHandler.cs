using AirQuality.OpenAQ.Contracts;
using AirQuality.OpenAQ.Data;
using FastEndpoints;

namespace AirQuality.OpenAQ.Integrations
{
    internal class GetSensorsByLocationIdHandler : ICommandHandler<GetSensorsByLocationIdCommand, List<SensorDTO>>
    {
        private readonly IOpenAQService _openAQService;
        private readonly ISensorRepository _sensorRepository;
        public GetSensorsByLocationIdHandler(IOpenAQService openAQService, ISensorRepository sensorRepository)
        {
            _openAQService = openAQService;
            _sensorRepository = sensorRepository;
        }

        public async Task<List<SensorDTO>> ExecuteAsync(GetSensorsByLocationIdCommand command, CancellationToken ct)
        {
            var sensors = await _sensorRepository.GetSensorsByLocationIdAsync(command.locationId);
            if (sensors is not null && sensors.Count > 0)
            {
                return sensors;
            }
            var response = await _openAQService.GetSensorsByLocationIdAsync(command.locationId);
            if (response is null || response.Results is null || response.Results.Count == 0)
            {
                return [];
            }
            foreach (var sensor in response.Results)
            {
                sensor.LocationId = command.locationId;
                sensor.LastUpdate = DateTime.UtcNow;
            }
            return response.Results;
        }
    }
}
