namespace AirQuality.Geocoding.Contracts
{
    public class GeoLocation
    {
        public int? Id { get; set; }
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<string> Boundingbox { get; set; }
        public List<GeoCodingInfo> GeoCodingInfos { get; set; }
    }
}
