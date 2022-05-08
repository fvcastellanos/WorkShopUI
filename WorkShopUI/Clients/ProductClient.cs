namespace WorkShopUI.Clients
{
    public class ProductClient : BaseHttpClient
    {
        public ProductClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}