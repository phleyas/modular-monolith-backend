using AirQuality.Geocoding.Contracts;
using AirQuality.Locations.Validations;
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
            var validation = new LocationsRequestValidation();
            var validationResult = await validation.ValidateAsync(req, ct);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddError(error.ErrorMessage);
                }
                ThrowIfAnyErrors();
                return;
            }

            var geo = await new GetCoordinatesByLocationCommand() { City = req.CityPurged!, Country = req.CountryPurged! }.ExecuteAsync();
            if (geo is null)
            {
                await Send.NotFoundAsync();
                return;
            }

            await PublishAsync(new UpdateGeoCodingInfoEvent()
            {
                City = req.CityPurged,
                Country = req.CountryPurged,
                GeoLocation = geo
            });

            if (!double.TryParse(geo.Lat, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) ||
             !double.TryParse(geo.Lon, NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
            {
                AddError("Could not parse Coordinates");
                ThrowIfAnyErrors();
                return;
            }

            var locations = await new GetLocationsByCoordinatesCommand() { latitude = lat, longuitude = lon, radius = 10000, limit = 10 }.ExecuteAsync();
            if (locations is null || locations.Count == 0)
            {
                AddError("Could not find any locations");
                ThrowIfAnyErrors();
                return;
            }

            await Send.OkAsync(new LocationsResponse() { Locations = locations });
        }
    }
}
