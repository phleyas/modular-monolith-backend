using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.OpenAQ
{
    public class GetLocationsByCoordinatesHandler : ICommandHandler<GetLocationsByCoordinatesCommand, OpenAQResponse<SensorLocationDTO>>
    {
        private readonly ISensorLocationsService _sensorLocationsService;
        public GetLocationsByCoordinatesHandler(ISensorLocationsService sensorLocationsService)
        {
            _sensorLocationsService = sensorLocationsService;
        }
        public async Task<OpenAQResponse<SensorLocationDTO>> ExecuteAsync(GetLocationsByCoordinatesCommand command, CancellationToken ct)
        {
            return await _sensorLocationsService.GetLocationsByCoordinatesAsync(command.latitude, command.longuitude, command.radius, command.limit); ;
        }
    }
}
