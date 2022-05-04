
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{

    public class CarBrandClient : BaseHttpClient
    {
        public CarBrandClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public SearchResponse<CarBrand> Search(int active, string name, int page, int size)
        {
            var url = $"{ClientConstants.CarBrandResource}?active={active}&name={name}&page={page}&size={size}";
            
            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    System.Console.WriteLine(responsePayload);

                    return JsonDeserialize<SearchResponse<CarBrand>>(responsePayload);
                }
            }

            throw new HttpRequestException("Oh no!!!");             
        }
    }

}