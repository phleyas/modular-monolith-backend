using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class CountryDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("datetimeFirst")]
        public DateTime? DatetimeFirst { get; set; }

        [JsonPropertyName("datetimeLast")]
        public DateTime? DatetimeLast { get; set; }

        [NotMapped]
        [JsonPropertyName("parameters")]
        public List<ParameterDTO> Parameters { get; set; }
    }
    public class ParameterDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("units")]
        public string Units { get; set; }

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }
    }
}
