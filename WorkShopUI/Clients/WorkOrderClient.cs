using LanguageExt;
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

        public SearchResponse<WorkOrder> Search(string text, string status, int page, int size)
        {
            var url = $"{ClientConstants.WorkOrderResource}?text={text}&status={status}&page={page}&size={size}";
            var accessToken = GetAccessToken();

            return Search<WorkOrder>(accessToken, url);            
        }

        public Option<WorkOrder> FindById(string id)
        {
            var url = $"{ClientConstants.WorkOrderResource}/{id}";
            var accessToken = GetAccessToken();

            return FindById<WorkOrder>(accessToken, id);
        }
    }
}