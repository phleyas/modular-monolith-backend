namespace AirQuality.Geocoding.Contracts
{
    public class UpdateGeoCodingInfoEvent
    {
        public GeoLocation GeoLocation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
