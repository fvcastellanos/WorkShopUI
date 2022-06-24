
using LanguageExt;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public class CarBrandClient : BaseHttpClient
    {
        public CarBrandClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public SearchResponse<CarBrand> Search(int active, string name, int page, int size)
        {
            var url = $"{ClientConstants.CarBrandResource}?active={active}&name={name}&page={page}&size={size}";

            var accessToken = GetAccessToken();
            return Search<CarBrand>(accessToken, url);
        }

        public void Add(CarBrand carBrand) 
        {
            var content = CreateStringContent(carBrand);
            var accessToken = GetAccessToken();

            Add(accessToken, ClientConstants.CarBrandResource, content);
        }

        public Option<CarBrand> FindById(string id)
        {
            var url = $"{ClientConstants.CarBrandResource}/{id}";
            var accessToken = GetAccessToken();

            return FindById<CarBrand>(accessToken, url);
        }

        public void Update(string id, CarBrand carBrand)
        {
            var url = $"{ClientConstants.CarBrandResource}/{id}";
            var content = CreateStringContent(carBrand);
            var accessToken = GetAccessToken();

            Update(accessToken, url, content);
        }
    }

}