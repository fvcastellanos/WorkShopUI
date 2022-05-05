using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class ResourceObject
    {
        [JsonPropertyName("links")]
        public IEnumerable<Link> Links { get; set; }

    }
}