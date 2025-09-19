using AirQuality.OpenAQ.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ILogger = Serilog.ILogger;

namespace AirQuality.OpenAQ
{
    public static class OpenAQModuleServiceExtensions
    {
        public static IServiceCollection AddOpenAQModuleServices(this IServiceCollection services,
              ConfigurationManager config, ILogger logger
         )
        {

            services.AddHttpClient<IOpenAQService, OpenAQService>((sp, client) =>
            {
                client.BaseAddress = new Uri("https://api.openaq.org/v3/");

                var cfg = sp.GetRequiredService<IConfiguration>();

                var apiKey = cfg["openaq_api_key"];
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                }
            });

            var defaultConnection = config["connection_strings_open_aq"];
            services.AddDbContext<OpenAQDbContext>(options =>
                           options.UseNpgsql(defaultConnection));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IParametersRepository, ParametersRepository>();


            logger.Information("{Module} module services registered", "OpenAQ");

            return services;
        }
    }
}
