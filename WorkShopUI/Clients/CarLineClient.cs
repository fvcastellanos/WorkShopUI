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

        public Option<CarLine> FindById(string carLineId)
        {
            try
            {

            }
            catch (Exception exception)
            {

                return null;
            }
        }
    }
}
