

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class ContactBase : CrudBase
    {

        protected ContactSearchView SearchView;

        protected PagedView<ContactView> SearchResponse;

        protected IEnumerable<ContactView> Contacts;

        protected ContactView ContactView;
        
        [Inject]
        protected ContactService ContactService { get; set; }

        protected override void OnInitialized()
        {
            SearchView = new ContactSearchView
            {
                Page = 0,
                Size = 25,
                Active = 1,
                Name = "",
                Type = "CUSTOMER"
            };

            HideAddModal();
            Search();
        }

        protected override void Add()
        {
            var result = ContactService.Add(ContactView);

            result.Match(right => {
                
                HideAddModal();
                Search();
            }, DisplayModalError);
        }

        protected override void DisplayPage(int pageNumber)
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            Contacts = new List<ContactView>();

            HideErrorMessage();

            var result = ContactService.Search(SearchView);

            result.Match(right => {

                // SearchResponse = right;
                Contacts = right;
            }, ShowErrorMessage);
        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }

        protected void GetContact(string id)
        {
            var holder = ContactService.FindById(id);

            
        }

        protected void ShowAddModal() 
        {
            ContactView = new ContactView
            {
                Active = "ACTIVE",
                Type = "CUSTOMER"
            };

            EditContext = new EditContext(ContactView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }
    }
}
