using FluentValidation;

namespace AirQuality.Api.Endpoints
{
    internal class LocationsRequestValidation : AbstractValidator<LocationsRequest>
    {
        public LocationsRequestValidation()
        {
            RuleFor(x => x.CityPurged)
                .NotNull().WithMessage("City can't be empty.")
                .MinimumLength(2).WithMessage("City is too short.")
                .MaximumLength(26).WithMessage("City is too long.")
                .OverridePropertyName("City");

            RuleFor(x => x.CountryPurged)
                .NotNull().WithMessage("Country is required.")
                .MinimumLength(2).WithMessage("Country is too short.")
                .MaximumLength(56).WithMessage("Country is too long.")
                .OverridePropertyName("Country");
        }
    }
}
