// using New

using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class CarBrand : ResourceObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }
    }
}