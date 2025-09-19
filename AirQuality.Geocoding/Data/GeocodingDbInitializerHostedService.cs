using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AirQuality.Geocoding.Data
{
    public sealed class GeocodingDbInitializerHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<GeocodingDbInitializerHostedService> _logger;

        public GeocodingDbInitializerHostedService(IServiceProvider services, ILogger<GeocodingDbInitializerHostedService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GeoCodingDBContext>();

            try
            {
                var pending = await db.Database.GetPendingMigrationsAsync(cancellationToken);
                if (pending.Any())
                {
                    _logger.LogInformation("Applying {Count} geocoding DB migration(s)...", pending.Count());
                    await db.Database.MigrateAsync(cancellationToken);
                }
                else
                {
                    _logger.LogInformation("No migrations found. Ensuring geocoding DB is created...");
                    await db.Database.EnsureCreatedAsync(cancellationToken);
                }

                _logger.LogInformation("Geocoding DB ready: {Database}", db.Database.GetDbConnection().Database);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Geocoding database. Connection: {ConnStr}",
                    db.Database.GetDbConnection().ConnectionString);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


    }
}