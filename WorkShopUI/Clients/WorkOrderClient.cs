using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{
    public class WorkOrderClient : BaseHttpClient
    {
        public WorkOrderClient(IHttpClientFactory httpClientFactory, 
                               IHttpContextAccessor httpContextAccessor) 
                               : base(httpClientFactory, httpContextAccessor)
        {
        }

        public SearchResponse<WorkOrder> Search(string number, string plateNumber, string status, int page, int size)
        {
            var url = $"{ClientConstants.WorkOrderResource}?number={number}&plateNumber={plateNumber}&status={status}&page={page}&size={size}";
            var accessToken = GetAccessToken();

            return Search<WorkOrder>(accessToken, url);            
        }
    }
}