using AirQuality.OpenAQ.Contracts;
using FastEndpoints;

namespace AirQuality.Api.Endpoints
{
    internal class GetParameters : Endpoint<ParametersRequest, ParametersResponse>
    {
        public override void Configure()
        {
            Get("/parameters");
            AllowAnonymous();
        }
        public override async Task HandleAsync(ParametersRequest req, CancellationToken ct)
        {

            var results = await new GetParameterByIdCommand() { id = req.Id }.ExecuteAsync();
            if (results is null)
            {
                await Send.NotFoundAsync();
                return;
            }

            await Send.OkAsync(new ParametersResponse() { Parameters = results });
        }
    }
}
