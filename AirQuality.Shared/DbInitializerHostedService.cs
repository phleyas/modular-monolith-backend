using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AirQuality.Shared
{
    public sealed class DbInitializerHostedService<TContext> : IHostedService where TContext : DbContext
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DbInitializerHostedService<TContext>> _logger;

        public DbInitializerHostedService(IServiceProvider services, ILogger<DbInitializerHostedService<TContext>> logger)
        {
            _services = services;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TContext>();

            try
            {
                var ctxName = typeof(TContext).Name;
                var pending = await db.Database.GetPendingMigrationsAsync(cancellationToken);
                if (pending.Any())
                {
                    _logger.LogInformation("Applying {Count} migration(s) for {Context}...", pending.Count(), ctxName);
                    await db.Database.MigrateAsync(cancellationToken);
                }
                else
                {
                    _logger.LogInformation("No migrations found for {Context}. Ensuring database is created...", ctxName);
                    await db.Database.EnsureCreatedAsync(cancellationToken);
                }

                _logger.LogInformation("{Context} DB ready: {Database}", ctxName, db.Database.GetDbConnection().Database);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize database for context {Context}. Connection: {ConnStr}",
                    typeof(TContext).Name, db.Database.GetDbConnection().ConnectionString);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}