using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public class ProductClient : BaseHttpClient
    {
        public ProductClient(IHttpClientFactory httpClientFactory,
                             IHttpContextAccessor httpContextAccessor)
                              : base(httpClientFactory, httpContextAccessor)
        {
        }

        public SearchResponse<Product> Search(int active, string name, string code, string type, int page, int size)
        {
            var url = $"{ClientConstants.ProductResource}?active={active}&name={name}&&code={code}&type={type}&page={page}&size={size}";
            var accessToken = GetAccessToken();

            return Search<Product>(accessToken, url);
        }
    }
}