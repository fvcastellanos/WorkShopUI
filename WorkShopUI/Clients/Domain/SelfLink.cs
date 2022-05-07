using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class SelfLink
    {
        [JsonPropertyName("self")]
        public Self Self { get; set; }
    }
}