using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class CarLine: ResourceObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }
    }

}