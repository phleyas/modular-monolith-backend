using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class SensorDTO
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

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
        [JsonPropertyName("datetime")]
        public DateDTO Datetime { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("coordinates")]
        public CoordinatesDTO Coordinates { get; set; }
    }
    public class SummaryDTO
    {
        [JsonPropertyName("min")]
        public double? Min { get; set; }

        [JsonPropertyName("q02")]
        public object Q02 { get; set; }

        [JsonPropertyName("q25")]
        public object Q25 { get; set; }

        [JsonPropertyName("median")]
        public object Median { get; set; }

        [JsonPropertyName("q75")]
        public object Q75 { get; set; }

        [JsonPropertyName("q98")]
        public object Q98 { get; set; }

        [JsonPropertyName("max")]
        public double? Max { get; set; }

        [JsonPropertyName("avg")]
        public double? Avg { get; set; }

        [JsonPropertyName("sd")]
        public object Sd { get; set; }
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
