
using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class Contact : ResourceObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("contact")]
        public string ContactName { get; set; }

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }
    }
}