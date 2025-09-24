using AirQuality.OpenAQ.Contracts;
using AirQuality.OpenAQ.Data;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace AirQuality.OpenAQ.Integrations
{
    internal class UpdateSensorsHandler : IEventHandler<UpdateSensorsEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateSensorsHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task HandleAsync(UpdateSensorsEvent eventModel, CancellationToken ct)
        {
            if (eventModel is null || eventModel.Sensors is null || eventModel.Sensors.Count == 0)
                return;

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
            foreach (var incoming in eventModel.Sensors)
            {
                await repo.UpdateSensorAsync(incoming);
            }
            await repo.SaveChangesAsync();
        }
    }
}
