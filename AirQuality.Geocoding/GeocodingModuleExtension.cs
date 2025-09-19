using AirQuality.Geocoding.Data;
using AirQuality.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ILogger = Serilog.ILogger;

namespace AirQuality.Geocoding
{
    public static class GeocodingModuleServiceExtensions
    {
        public static IServiceCollection AddGeocodingModuleServices(this IServiceCollection services,
              ConfigurationManager config, ILogger logger
         )
        {

            services.AddHttpClient<IGeocodingService, NominatimGeocodingService>((sp, client) =>
            {
                client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");

                var cfg = sp.GetRequiredService<IConfiguration>();

                var userAgent = cfg["nominatim_user_agent"];
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                }

                var referer = cfg["nominatim_referer"];
                if (!string.IsNullOrWhiteSpace(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var refUri))
                {
                    client.DefaultRequestHeaders.Referrer = refUri;
                }
            });



            var defaultConnection = config["connection_strings_geocoding"];
            services.AddDbContext<GeoCodingDBContext>(options =>
                           options.UseNpgsql(defaultConnection));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHostedService<DbInitializerHostedService<GeoCodingDBContext>>();

            services.AddScoped<IGeoCodingInfoRepository, GeoCodingInfoRepository>();


            logger.Information("{Module} module services registered", "Geocoding");

            return services;
        }
    }
}
