
using LanguageExt;
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

                    return JsonDeserialize<SearchResponse<CarBrand>>(responsePayload);
                }
            }

            throw new HttpRequestException("Unable to retrieve search results");             
        }

        public CarBrand Add(CarBrand carBrand) {

            var content = CreateStringContent(carBrand);

            using (var response = HttpClient.PostAsync(ClientConstants.CarBrandResource, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Unable to add Car Brand");
                }

                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                return JsonDeserialize<CarBrand>(responsePayload);
            }
        }

        public Option<CarBrand> FindById(string id)
        {
            var url = $"{ClientConstants.CarBrandResource}/{id}";

            return FindById<CarBrand>("", url, $"Unable to find Car brand with id: {id}");
        }

        public void Update(string id, CarBrand carBrand)
        {

            var url = $"{ClientConstants.CarBrandResource}/{id}";
            var content = CreateStringContent(carBrand);

            Update("", url, content, $"Unable to update car brand with id: {id}");
        }
    }

}