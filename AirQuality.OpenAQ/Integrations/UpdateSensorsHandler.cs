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
                var existing = await repo.GetSensorByIdAsync(incoming.Id);

                if (existing is not null)
                {
                    if (existing.LastUpdate.HasValue && existing.LastUpdate.Value <= DateTime.UtcNow.AddHours(-1))
                    {
                        // older than 1 hour -> remove
                        await repo.RemoveSensorAsync(existing.Id);
                        continue;
                    }
                }
                else
                {

                    await repo.UpdateSensorAsync(incoming);
                }
            }
            await repo.SaveChangesAsync();
        }
    }
}
