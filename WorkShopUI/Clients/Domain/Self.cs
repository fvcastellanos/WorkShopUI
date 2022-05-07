using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class Self
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }
}