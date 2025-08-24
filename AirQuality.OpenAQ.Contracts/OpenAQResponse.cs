using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class OpenAQResponse<TResult>
    {
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("results")]
        public List<TResult> Results { get; set; }
    }
    public class Meta
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("found")]
        public int Found { get; set; }
    }
}
