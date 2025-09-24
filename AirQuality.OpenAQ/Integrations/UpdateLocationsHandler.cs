using AirQuality.OpenAQ.Contracts;
using AirQuality.OpenAQ.Data;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace AirQuality.OpenAQ.Integrations
{
    internal class UpdateLocationsHandler : IEventHandler<UpdateLocationsEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateLocationsHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(UpdateLocationsEvent eventModel, CancellationToken ct)
        {
            if (eventModel is null || eventModel.Locations is null || eventModel.Locations.Count == 0)
                return;

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ILocationsRepository>();

            foreach (var incoming in eventModel.Locations)
            {
                await repo.UpdateLocationAsync(incoming);

            }

            await repo.SaveChangesAsync();
        }
    }
}
