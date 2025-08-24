using AirQuality.Geocoding;
using AirQuality.OpenAQ;
using FastEndpoints;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder();


builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);



builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddHttpLogging(o => { });

builder.Services.AddFastEndpoints(o =>
{
    o.SourceGeneratorDiscoveredTypes.AddRange(AirQuality.Locations.DiscoveredTypes.All);
    o.SourceGeneratorDiscoveredTypes.AddRange(AirQuality.Geocoding.DiscoveredTypes.All);
    o.SourceGeneratorDiscoveredTypes.AddRange(AirQuality.OpenAQ.DiscoveredTypes.All);
});

builder.Services.AddHttpClient();

builder.Services.AddGeocodingModuleServices(builder.Configuration, logger);
builder.Services.AddOpenAQModuleServices(builder.Configuration, logger);

var app = builder.Build();

app.UseFastEndpoints();

app.Run();
