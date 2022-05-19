
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{

    public class ContactClient : BaseHttpClient
    {
        public ContactClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public SearchResponse<Contact> Search(int active, string name, string code, string type, int page, int size)
        {
            var url = $"{ClientConstants.ContactResource}?active={active}&name={name}&&code={code}&type={type}&page={page}&size={size}";

            return Find<Contact>("", url, "Unable to retrieve search results");
            
        }
    }
}