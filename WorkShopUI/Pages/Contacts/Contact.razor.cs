

using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain.Views;
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
                Type = "C"
            };

            ContactView = new ContactView();
            HideAddModal();
            Search();

            base.OnInitialized();
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
            SearchView.Page = pageNumber;
            Search();
            StateHasChanged();
        }

        protected override void Search()
        {
            Contacts = new List<ContactView>();

            HideErrorMessage();

            var result = ContactService.Search(SearchView);

            result.Match(right => {

                SearchResponse = right;
                Contacts = right.Content;                
            }, ShowErrorMessage);
        }

        protected override void Update()
        {
            var result = ContactService.Update(ContactView);

            result.Match(right => {

                HideAddModal();
                Search();

            }, DisplayModalError);
        }

        protected void GetContact(string id)
        {
            var holder = ContactService.FindById(id);

            holder.Match(ShowEditModal, 
                () => ShowErrorMessage("No encontro el contacto seleccionado"));
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

        protected void OnReadData(DataGridReadDataEventArgs<ContactView> eventArgs)
        {
            SearchView.Page = eventArgs.Page - 1;
            Search();
        }

        // ------------------------------------------------------------------------------------------
        private void ShowEditModal(ContactView view)
        {
            ContactView = view;
            EditContext = new EditContext(ContactView);
            ShowEditModal();
        }
    }
}
