
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain.Views;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class CarBrandBase: CrudBase
    {
        [Inject]
        protected CarBrandService Service { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected SearchView SearchView;
        protected IEnumerable<CarBrandView> CarBrands;
        protected CarBrandView CarBrandView;

        protected override async Task OnInitializedAsync()
        {
            SearchView = new SearchView
            {
                Page = 0,
                Size = 25,
                Active = 1,
                Name = ""
            };

            HideAddModal();
            await SearchAsync();
        }

        protected override async Task SearchAsync()
        {
            CarBrands = new List<CarBrandView>();
            HideErrorMessage();

            var result = await Service.SearchAsync(SearchView);
            result.Match(right => {

                CarBrands = right;

            }, ShowErrorMessage); 

        }
        
        protected override async Task DisplayPageAsync(int pageNumber)
        {
            SearchView.Page = pageNumber;
            await SearchAsync();
            StateHasChanged();
        }
        protected void ShowAddModal()
        {
            CarBrandView = new CarBrandView()
            {
                Active = "ACTIVE"
            };

            EditContext = new EditContext(CarBrandView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected async Task GetBrandAsync(string id)
        {
            var holder = await Service.FindByIdAsync(id);

            holder.Match(ShowEditModal,
                () => ShowErrorMessage($"No se encontro informacion del fabricante con id: {id}"));
        }

        protected void GetLines(string id, string name)
        {
            NavigationManager.NavigateTo($"/car-brands/{id}/lines?CarBrandName={name}");
        }

        protected override async Task UpdateAsync()
        {
            var result = await Service.UpdateAsync(CarBrandView);

            result.Match(async right => {

                HideAddModal();
                await SearchAsync();
            }, DisplayModalError);
        }

        protected override async Task AddAsync()
        {
            var result = await Service.AddAsync(CarBrandView);

            result.Match(async right => {

                HideAddModal();
                await SearchAsync();
            }, DisplayModalError);
        }

        // ------------------------------------------------------------------------------------

        private void ShowEditModal(CarBrandView view)
        {
            CarBrandView = view;
            EditContext = new EditContext(CarBrandView);
            ShowEditModal();
        }
    }
}