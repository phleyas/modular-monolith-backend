using System.Text.Json.Serialization;

namespace AirQuality.OpenAQ.Contracts
{
    public class DateDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("utc")]
        public DateTime? Utc { get; set; }
        [JsonPropertyName("local")]
        public DateTime? Local { get; set; }

    }
}
