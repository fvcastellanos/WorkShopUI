using System.Text;
using System.Text.Json;
using LanguageExt;
using Microsoft.AspNetCore.Authentication;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public abstract class BaseHttpClient
    {
        private const string AuthorizationHeaderName = "Authorization";

        protected readonly HttpClient HttpClient;

        protected readonly HttpContext HttpContext;
        protected readonly JsonSerializerOptions JsonSerializerOptions;

        public BaseHttpClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClientFactory.CreateClient(ClientConstants.ClientName);

            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            HttpContext = httpContextAccessor.HttpContext;
        }

        protected async Task<string> GetAccessTokenAsync()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }

        protected string GetAccessToken()
        {
            return HttpContext.GetTokenAsync("access_token")
                .Result;
        }

        protected T JsonDeserialize<T> (string jsonPayload)
        {
            return JsonSerializer.Deserialize<T>(jsonPayload, JsonSerializerOptions);
        }

        protected void AddAuthenticationHeader(string token) 
        {            
            if (HttpClient.DefaultRequestHeaders.Contains(AuthorizationHeaderName))
            {
                HttpClient.DefaultRequestHeaders.Remove(AuthorizationHeaderName);
            }            

            HttpClient.DefaultRequestHeaders.Add(AuthorizationHeaderName, "Bearer " + token);
        }

        protected StringContent CreateStringContent(object obj)
        {
            var payload = JsonSerializer.Serialize(obj);
            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

        protected async Task<SearchResponse<T>> SearchAsync<T>(string token, string url)
        {
            AddAuthenticationHeader(token);

            using (var response = await HttpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = await response.Content
                        .ReadAsStringAsync();

                    return JsonDeserialize<SearchResponse<T>>(responsePayload);
                }

                var apiError = await response.Content
                    .ReadFromJsonAsync<ApiError>();

                throw new HttpRequestException(apiError.Message);            
            }
        }

        protected SearchResponse<T> Search<T>(string token, string url)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content
                        .ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<SearchResponse<T>>(responsePayload);
                }

                var apiError = GetApiError(response);
                throw new HttpRequestException(apiError.Message);            
            }
        }

        protected async Task<Option<T>> FindByIdAsync<T>(string token, string url)
        {
            AddAuthenticationHeader(token);

            using (var response = await HttpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = await response.Content
                        .ReadAsStringAsync();

                    return JsonDeserialize<T>(responsePayload);
                }

                if (response.StatusCode.Equals(404))
                {
                    return Option<T>.None;
                }

                var apiError = await GetApiErrorAsync(response); 
                throw new HttpRequestException(apiError.Message);
            }
        }

        protected Option<T> FindById<T>(string token, string url)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content
                        .ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<T>(responsePayload);
                }

                if (response.StatusCode.Equals(404))
                {
                    return Option<T>.None;
                }

                var apiError = GetApiError(response); 
                throw new HttpRequestException(apiError.Message);
            }
        }

        protected async Task AddAsync(string token, string url, StringContent content)
        {
            AddAuthenticationHeader(token);

            using (var response = await HttpClient.PostAsync(url, content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var apiError = await GetApiErrorAsync(response);
                    throw new HttpRequestException(apiError.Message);
                }
            }
        }

        protected void Add(string token, string url, StringContent content)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.PostAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    var apiError = GetApiError(response);
                    throw new HttpRequestException(apiError.Message);
                }
            }
        }

        protected async Task UpdateAsync(string token, string url, StringContent content)
        {
            AddAuthenticationHeader(token);

            using (var response = await HttpClient.PutAsync(url, content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var apiError = await GetApiErrorAsync(response);
                    throw new HttpRequestException(apiError.Message);
                }
            }
        }

        protected void Update(string token, string url, StringContent content)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.PutAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    var apiError = GetApiError(response);
                    throw new HttpRequestException(apiError.Message);
                }
            }
        }

        private async Task<ApiError> GetApiErrorAsync(HttpResponseMessage response)
        {
            return await response.Content
                .ReadFromJsonAsync<ApiError>();
        }

        private ApiError GetApiError(HttpResponseMessage response)
        {
            return response.Content
                .ReadFromJsonAsync<ApiError>()
                .Result;
        }

    }
}