using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class Product : ResourceObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }
    }
}