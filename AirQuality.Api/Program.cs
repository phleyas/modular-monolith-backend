using AirQuality.Geocoding;
using AirQuality.OpenAQ;
using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");


var builder = WebApplication.CreateBuilder();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://localhost:4200",
            "https://ignaciokoestner.de",
            "https://www.ignaciokoestner.de")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);



builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddHttpLogging(o => { });

builder.Services.AddFastEndpoints(o =>
{
    o.SourceGeneratorDiscoveredTypes.AddRange(AirQuality.Locations.DiscoveredTypes.All);
    o.SourceGeneratorDiscoveredTypes.AddRange(AirQuality.Geocoding.DiscoveredTypes.All);
    o.SourceGeneratorDiscoveredTypes.AddRange(AirQuality.OpenAQ.DiscoveredTypes.All);
}).SwaggerDocument(o => o.ShortSchemaNames = true);

builder.Services.AddHttpClient();

builder.Services.AddGeocodingModuleServices(builder.Configuration, logger);
builder.Services.AddOpenAQModuleServices(builder.Configuration, logger);

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
}).UseSwaggerGen();

app.Run();
