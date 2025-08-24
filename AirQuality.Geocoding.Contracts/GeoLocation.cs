namespace AirQuality.Geolocation.Contracts
{
    public class GeoLocation
    {
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<string> Boundingbox { get; set; }
    }
}
