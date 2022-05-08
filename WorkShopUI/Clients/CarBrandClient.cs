
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

            return Find<CarBrand>("", url, "Unable to retrieve search results");
        }

        public CarBrand Add(CarBrand carBrand) {

            var content = CreateStringContent(carBrand);
            Add("", ClientConstants.CarBrandResource, content, "Unable to add Car Brand");

            return carBrand;
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