using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class ApiError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}