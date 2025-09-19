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
                var existing = await repo.GetLocationByIdAsync(incoming.Id);

                if (existing is not null)
                {
                    if (existing.LastUpdate.HasValue && existing.LastUpdate.Value <= DateTime.UtcNow.AddHours(-1))
                    {
                        // older than 1 hour -> remove
                        await repo.RemoveLocationAsync(existing.Id);
                        continue;
                    }

                    if (!existing.LastUpdate.HasValue)
                    {
                        // not set -> set to now
                        existing.LastUpdate = DateTime.UtcNow;
                    }
                    // Do nothing when <= 1 hour old
                }
                else
                {
                    // Not in DB: if LastUpdate not set, set and add
                    if (!incoming.LastUpdate.HasValue)
                    {
                        incoming.LastUpdate = DateTime.UtcNow;
                        await repo.UpdateLocationsAsync(new List<LocationDTO> { incoming });
                    }
                    // If LastUpdate is set on incoming, do nothing per requirement
                }
            }

            await repo.SaveChangesAsync();
        }
    }
}
