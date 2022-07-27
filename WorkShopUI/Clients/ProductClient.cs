using LanguageExt;
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

        public SearchResponse<Product> Search(int active, string text, string type, int page, int size)
        {
            var url = $"{ClientConstants.ProductResource}?active={active}&text={text}&type={type}&page={page}&size={size}";
            var accessToken = GetAccessToken();

            return Search<Product>(accessToken, url);
        }

        public void Add(Product product)
        {
            var content = CreateStringContent(product);
            var accessToken = GetAccessToken();

            Add(accessToken, ClientConstants.ProductResource, content);
        }

        public Option<Product> FindById(string id)
        {
            var url = $"{ClientConstants.ProductResource}/{id}";
            var accessToken = GetAccessToken();

            return FindById<Product>(accessToken, url);
        }

        public void Update(string id, Product product)
        {
            var url = $"{ClientConstants.ProductResource}/{id}";
            var content = CreateStringContent(product);
            var accessToken = GetAccessToken();

            Update(accessToken, url, content);
        }
    }
}