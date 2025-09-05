using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class MeasurementDTO
    {
        [JsonPropertyName("datetime")]
        public DateDTO Datetime { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("coordinates")]
        public CoordinatesDTO Coordinates { get; set; }

        [JsonPropertyName("sensorsId")]
        public int? SensorsId { get; set; }

        [JsonPropertyName("locationsId")]
        public int? LocationsId { get; set; }
    }
}
