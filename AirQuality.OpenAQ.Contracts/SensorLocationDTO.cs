using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class SensorLocationDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("locality")]
        public string? Locality { get; set; }
        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
        [JsonPropertyName("country")]
        public CountryDTO? Country { get; set; }
        [JsonPropertyName("owner")]
        public IdNameDTO? Owner { get; set; }
        [JsonPropertyName("provider")]
        public IdNameDTO? Provider { get; set; }
        [JsonPropertyName("isMobile")]
        public bool IsMobile { get; set; }
        [JsonPropertyName("isMonitor")]
        public bool IsMonitor { get; set; }
        [JsonPropertyName("instruments")]
        public List<IdNameDTO>? Instruments { get; set; }
        [JsonPropertyName("sensors")]
        public List<SensorDTO>? Sensors { get; set; }
        [JsonPropertyName("coordinates")]
        public CoordinatesDTO? Coordinates { get; set; }
        [JsonPropertyName("licenses")]
        public List<LicenceDTO>? Licenses { get; set; }
        [JsonPropertyName("bounds")]
        public List<double>? Bounds { get; set; }
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
        [JsonPropertyName("datetimeFirst")]
        public DateDTO? DatetimeFirst { get; set; }
        [JsonPropertyName("datetimeLast")]
        public DateDTO? DatetimeLast { get; set; }
    }
    public class CoordinatesDTO
    {
        public int Id { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
    public class SensorDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("parameter")]
        public ParameterDTO? Parameter { get; set; }
    }
    public class IdNameDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
    public class DateDTO
    {
        public int Id { get; set; }
        [JsonPropertyName("utc")]
        public DateTime? Utc { get; set; }
        [JsonPropertyName("local")]
        public DateTime? Local { get; set; }

    }
    public class LicenceDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("attribution")]
        public AttributionDTO? Attribution { get; set; }
        [JsonPropertyName("dateFrom")]
        public DateTime? DateFrom { get; set; }
        [JsonPropertyName("dateTo")]
        public DateTime? DateTo { get; set; }
    }
    public class AttributionDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
