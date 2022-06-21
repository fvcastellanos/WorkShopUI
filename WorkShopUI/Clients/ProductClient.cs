using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public class ProductClient : BaseHttpClient
    {
        public ProductClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public SearchResponse<Product> Search(int active, string name, string code, string type, int page, int size)
        {
            var url = $"{ClientConstants.ProductResource}?active={active}&name={name}&&code={code}&type={type}&page={page}&size={size}";

            return Find<Product>("", url, "Unable to retrieve search results");            
        }
    }
}