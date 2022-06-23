using System.Text.Json.Serialization;

namespace WorkShopUI.Authentication.Domain
{
    public class AuthError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}