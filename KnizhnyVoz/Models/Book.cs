using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KnizhnyVoz.Models
{
    public class Book
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("imageUri")]
        public string ImageUri { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
