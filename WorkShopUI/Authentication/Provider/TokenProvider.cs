using LanguageExt;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WorkShopUI.Authentication.Domain;

namespace WorkShopUI.Authentication.Provider
{
    public class TokenProvider
    {
        // private const string AuthResponseKeyName = "authInfo";
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public TokenProvider(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task StoreTokenInformationAsync(string userId, AuthResponse authResponse)
        {
            await _protectedLocalStorage.SetAsync(userId, authResponse);
        }

        public async Task<Option<AuthResponse>> GetTokenInformationAsync(string userId)
        {
            var result = await _protectedLocalStorage.GetAsync<AuthResponse>(userId);

            if (result.Success)
            {
                return result.Value;
            }

            return null;
        }

        public async Task<Option<string>> GetStoredTokenAsync(string userId)
        {
            var holder = await GetTokenInformationAsync(userId);

            return holder.Map(info => info.AccessToken);
        }

        public async Task DeleteTokenInformationAsync(string userId)
        {
            await _protectedLocalStorage.DeleteAsync(userId);
        }
    }
}