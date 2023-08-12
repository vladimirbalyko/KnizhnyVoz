using System.Text.Json.Serialization;

namespace KnizhnyVoz.Models
{
    public class AudioFile
    {
        [JsonPropertyName("blocked")]
        public bool Blocked { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
