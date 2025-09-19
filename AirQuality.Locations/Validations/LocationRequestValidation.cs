using AirQuality.Locations.Endpoints;
using FluentValidation;

namespace AirQuality.Locations.Validations
{
    internal class LocationsRequestValidation : AbstractValidator<LocationsRequest>
    {
        public LocationsRequestValidation()
        {
            RuleFor(x => x.CityPurged).NotEmpty().NotNull().WithMessage("City is required.");
            RuleFor(x => x.CountryPurged).NotEmpty().NotNull().WithMessage("Country is required.");
        }
    }
}
