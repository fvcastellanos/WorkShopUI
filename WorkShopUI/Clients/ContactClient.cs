
using LanguageExt;
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

        public void Add(Contact contact)
        {
            var content = CreateStringContent(contact);
            Add("", ClientConstants.ContactResource, content, $"Unable to add contact with name: {contact.Name}");
        }

        public Option<Contact> FindById(string id)
        {            
            var url = $"{ClientConstants.ContactResource}/{id}";
            return FindById<Contact>("", url, "Contact {id} not found");
        }

        public void Update(string id, Contact contact)
        {
            var url = $"{ClientConstants.ContactResource}/{id}";
            var content = CreateStringContent(contact);
            Update("", url, content, "");
        }
    }
}