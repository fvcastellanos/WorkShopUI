using WorkShopUI.Authentication.Domain;

namespace WorkShopUI.Authentication
{
    public class AuthClient
    {
        private readonly HttpClient _httpClient;

        private readonly string _clientId;

        private readonly string _clientSecret;

        private readonly string _audience;

        public AuthClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient(AuthenticationConstants.AuthClientName);

            _clientId = configuration["Auth0:ClientId"];
            _clientSecret = configuration["Auth0:ClientSecret"];
            _audience = configuration["Auth0:Audience"];
        }

        public async Task<AuthResponse> PerformAuthentication(string username, string password)
        {
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", username },
                { "password", password },
                { "scope", "openid" },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "audience", _audience },
            };

            var content = new FormUrlEncodedContent(parameters);

            using (var response = await _httpClient.PostAsync($"/oauth/token/", content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var apiError = await response.Content
                        .ReadFromJsonAsync<AuthError>();

                    throw new HttpRequestException(apiError.Error + " - " + apiError.ErrorDescription);
                }

                return await response.Content
                    .ReadFromJsonAsync<AuthResponse>();
            }
        }

        public async Task<AuthUserInfo> GetUserInfoAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            using (var response = await _httpClient.GetAsync("/userinfo"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var apiError = await response.Content.ReadFromJsonAsync<AuthError>();
                    throw new HttpRequestException(apiError.Error + " - " + apiError.ErrorDescription);
                }

                return await response.Content
                    .ReadFromJsonAsync<AuthUserInfo>();
            }
        }

    }
}