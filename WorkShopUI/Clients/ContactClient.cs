
using LanguageExt;
using WorkShopUI.Clients.Domain;

namespace WorkShopUI.Clients
{

    public class ContactClient : BaseHttpClient
    {
        public ContactClient(IHttpClientFactory httpClientFactory,
                             IHttpContextAccessor httpContextAccessor) 
                             : base(httpClientFactory, httpContextAccessor)
        {
        }

        public SearchResponse<Contact> Search(int active, string name, string code, string type, int page, int size)
        {
            var url = $"{ClientConstants.ContactResource}?active={active}&name={name}&&code={code}&type={type}&page={page}&size={size}";
            var accessToken = GetAccessToken();

            return Search<Contact>(accessToken, url);            
        }

        public void Add(Contact contact)
        {
            var content = CreateStringContent(contact);
            var accessToken = GetAccessToken();

            Add(accessToken, ClientConstants.ContactResource, content);
        }

        public Option<Contact> FindById(string id)
        {            
            var url = $"{ClientConstants.ContactResource}/{id}";
            var accessToken = GetAccessToken();

            return FindById<Contact>(accessToken, url);
        }

        public void UpdateAsync(string id, Contact contact)
        {
            var url = $"{ClientConstants.ContactResource}/{id}";
            var content = CreateStringContent(contact);
            var accessToken = GetAccessToken();

            Update(accessToken, url, content);
        }
    }
}