using LanguageExt;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public class CarLineClient : BaseHttpClient
    {
        public CarLineClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public SearchResponse<CarLine> Search(string brandId, int active, string name, int page, int size)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, brandId)}?active={active}&name={name}&size={size}";

            return Find<CarLine>("", url, $"Unable to retrieve car lines for brandId: {brandId}");
        }

        public void Add(string brandId, CarLine carLine)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, brandId)}";
            
            var content = CreateStringContent(carLine);
            Add("", url, content, $"Unable to add a new car line for brandId: {brandId}");
        }

        public Option<CarLine> FindById(string carBrandId, string carLineId)
        {

            var url = $"{String.Format(ClientConstants.CarLineResource, carBrandId)}/{carLineId}";

            return FindById<CarLine>("", url, $"Unable to retrieve car_line_id: {carLineId}");
        }

        public void Update(string brandId, string lineId, CarLine carLine)
        {
            var url = $"{String.Format(ClientConstants.CarLineResource, brandId)}/{lineId}";
            
            var content = CreateStringContent(carLine);
            Update("", url, content, $"Unable to update car_line_id: {lineId}");
        }
    }
}
