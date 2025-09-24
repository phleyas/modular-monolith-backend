using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class SensorDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parameter")]
        public ParameterDTO? Parameter { get; set; }

        [JsonPropertyName("datetimeFirst")]
        public DateDTO? DatetimeFirst { get; set; }

        [JsonPropertyName("datetimeLast")]
        public DateDTO? DatetimeLast { get; set; }

        [JsonPropertyName("coverage")]
        public CoverageDTO? Coverage { get; set; }

        [JsonPropertyName("latest")]
        public LatestDTO? Latest { get; set; }

        [JsonPropertyName("summary")]
        public SummaryDTO? Summary { get; set; }
    }
    public class LatestDTO
    {
        // Required marker so EF can detect existence even if all other props are null
        [JsonIgnore]
        public bool HasValueMarker { get; set; } = true;

        [JsonPropertyName("datetime")]
        public DateDTO? Datetime { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("coordinates")]
        public CoordinatesDTO? Coordinates { get; set; }
    }
    public class SummaryDTO
    {
        [JsonPropertyName("min")]
        public double? Min { get; set; }

        [JsonPropertyName("q02")]
        public double? Q02 { get; set; }

        [JsonPropertyName("q25")]
        public double? Q25 { get; set; }

        [JsonPropertyName("median")]
        public double? Median { get; set; }

        [JsonPropertyName("q75")]
        public double? Q75 { get; set; }

        [JsonPropertyName("q98")]
        public double? Q98 { get; set; }

        [JsonPropertyName("max")]
        public double? Max { get; set; }

        [JsonPropertyName("avg")]
        public double? Avg { get; set; }

        [JsonPropertyName("sd")]
        public double? Sd { get; set; }
    }
    public class CoverageDTO
    {
        [JsonPropertyName("expectedCount")]
        public int? ExpectedCount { get; set; }

        [JsonPropertyName("expectedInterval")]
        public string ExpectedInterval { get; set; }

        [JsonPropertyName("observedCount")]
        public int? ObservedCount { get; set; }

        [JsonPropertyName("observedInterval")]
        public string ObservedInterval { get; set; }

        [JsonPropertyName("percentComplete")]
        public double? PercentComplete { get; set; }

        [JsonPropertyName("percentCoverage")]
        public double? PercentCoverage { get; set; }

        [JsonPropertyName("datetimeFrom")]
        public DateDTO DatetimeFrom { get; set; }

        [JsonPropertyName("datetimeTo")]
        public DateDTO DatetimeTo { get; set; }
    }
}
