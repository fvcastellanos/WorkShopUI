using LanguageExt;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public class CarLineClient : BaseHttpClient
    {
        public CarLineClient(IHttpClientFactory httpClientFactory, 
                             IHttpContextAccessor httpContextAccessor)
                              : base(httpClientFactory, httpContextAccessor)
        {
        }

        public SearchResponse<CarLine> Search(string brandId, int active, string name, int page, int size)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, brandId)}?active={active}&name={name}&size={size}";
            var accessToken = GetAccessToken();

            return Search<CarLine>(accessToken, url);
        }

        public void Add(string brandId, CarLine carLine)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, brandId)}";            
            var content = CreateStringContent(carLine);
            var accessToken = GetAccessToken();

           Add(accessToken, url, content);
        }

        public Option<CarLine> FindById(string carBrandId, string carLineId)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, carBrandId)}/{carLineId}";
            var accessToken = GetAccessToken();

            return FindById<CarLine>(accessToken, url);
        }

        public void Update(string brandId, string lineId, CarLine carLine)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, brandId)}/{lineId}";
            var accessToken = GetAccessToken();            
            var content = CreateStringContent(carLine);

            Update(accessToken, url, content);
        }
    }
}
