using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain.Views;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class ContactBase : CrudBase
    {
        protected ContactSearchView SearchView;

        protected IEnumerable<ContactView> Contacts;

        protected ContactView ContactView;
        
        [Inject]
        protected ContactService ContactService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SearchView = new ContactSearchView
            {
                Page = 0,
                Size = 25,
                Active = 1,
                Text = "",
                Name = "",
                Type = "CUSTOMER"
            };

            HideAddModal();
            await SearchAsync();
        }

        protected override async Task AddAsync()
        {
            var result = await ContactService.AddAsync(ContactView);

            result.Match(async right => {
                
                HideAddModal();
                await SearchAsync();
            }, DisplayModalError);
        }

        protected override async Task DisplayPageAsync(int pageNumber)
        {
            throw new NotImplementedException();
        }

        protected override async Task SearchAsync()
        {
            Contacts = new List<ContactView>();

            HideErrorMessage();

            var result = await ContactService.SearchAsync(SearchView);

            result.Match(right => {
                Contacts = right;
            }, ShowErrorMessage);
        }

        protected override async Task UpdateAsync()
        {
            throw new NotImplementedException();
        }

        protected async Task GetContact(string id)
        {
            var holder = await ContactService.FindByIdAsync(id);

            
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
