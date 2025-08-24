using AirQuality.Geolocation.Contracts;
using AirQuality.OpenAQ.Contracts;
using FastEndpoints;
using System.Globalization;


namespace AirQuality.Locations.Endpoints
{
    public class GetLocations : Endpoint<LocationsRequest, LocationsResponse>
    {
        public override void Configure()
        {
            Get("/locations");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LocationsRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Country) || string.IsNullOrWhiteSpace(req.City))
            {
                AddError("Country and City are required.");
                ThrowIfAnyErrors();
                return;
            }
            var geo = await new GetCoordinatesByLocationCommand() { City = req.City!, Country = req.Country! }.ExecuteAsync<GeoLocation>();
            if (geo is null)
            {
                await Send.NotFoundAsync();
                return;
            }

            if (!double.TryParse(geo.Lat, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) ||
             !double.TryParse(geo.Lon, NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
            {
                AddError("Could not parse Coordinates");
                ThrowIfAnyErrors();
                return;
            }

            var openAQ = await new GetLocationsByCoordinatesCommand() { latitude = lat, longuitude = lon, radius = 10000, limit = 10 }.ExecuteAsync();
            if (openAQ is null)
            {
                AddError("Could not find any locations");
                ThrowIfAnyErrors();
                return;
            }

            await Send.OkAsync(new LocationsResponse() { Locations = openAQ.Results });
        }
    }
}
