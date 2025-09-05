using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.Locations.Endpoints
{
    public class GetLatestMeasurements : Endpoint<LatestMeasurementsRequest, LatestMeasurementsResponse>
    {
        public override void Configure()
        {
            Get("/locations/{LocationId}/latest");
            AllowAnonymous();
        }
        public override async Task HandleAsync(LatestMeasurementsRequest req, CancellationToken ct)
        {
            if (req.LocationId is null)
            {
                AddError("location is required");
                ThrowIfAnyErrors();
                return;
            }
            var meassurements = await new GetLatestMeassurementsByLocationIdCommand() { locationId = req.LocationId.Value }.ExecuteAsync();
            if (meassurements is null || meassurements.Count == 0)
            {
                AddError("Could not find any meassurements");
                ThrowIfAnyErrors();
                return;
            }
            await Send.OkAsync(new LatestMeasurementsResponse() { Measurements = meassurements });
        }
    }
}
