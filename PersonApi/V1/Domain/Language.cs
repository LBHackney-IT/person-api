using System.Text.Json.Serialization;

namespace PersonApi.V1.Domain
{
    public class Language
    {
        [JsonPropertyName("language")]
        public string Name { get; set; }
        public bool IsPrimary { get; set; }
    }
}
