namespace AirQuality.Geocoding.Contracts
{
    public class GeoCodingInfo
    {
        public int Id { get; set; }
        public required DateTime LastUpdate { get; set; }
        public int GeoLocationId { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public GeoLocation GeoLocation { get; set; }
    }
}
