using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.Api.Endpoints
{
    public class GetSensors : Endpoint<SensorsRequest, SensorsResponse>
    {
        public override void Configure()
        {
            Get("/sensors");
            AllowAnonymous();
        }
        public override async Task HandleAsync(SensorsRequest req, CancellationToken ct)
        {
            if (req.LocationId is null)
            {
                AddError("location is required");
                ThrowIfAnyErrors();
                return;
            }
            var sensors = await new GetSensorsByLocationIdCommand() { locationId = req.LocationId.Value }.ExecuteAsync();
            if (sensors is null || sensors.Count == 0)
            {
                AddError("Could not find any meassurements");
                ThrowIfAnyErrors();
                return;
            }
            await Send.OkAsync(new SensorsResponse() { Sensors = sensors });
        }
    }
}
