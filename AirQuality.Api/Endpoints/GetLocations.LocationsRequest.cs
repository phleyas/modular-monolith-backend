using FastEndpoints;

namespace AirQuality.Api.Endpoints
{
    public class LocationsRequest
    {
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                CountryPurged = value.Trim().ToLower();
            }
        }
        [HideFromDocs]
        public string? CountryPurged { get; private set; }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                CityPurged = value.Trim().ToLower();
            }
        }
        [HideFromDocs]
        public string? CityPurged { get; private set; }

    }
}
