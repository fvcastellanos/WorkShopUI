
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

        protected PagedView<CarBrandView> SearchResponse;
        protected SearchView SearchView;
        protected IEnumerable<CarBrandView> CarBrands;
        protected CarBrandView CarBrandView;

        protected override void OnInitialized()
        {
            SearchView = new SearchView
            {
                Page = 0,
                Size = 25,
                Active = 1,
                Name = ""
            };

            HideAddModal();
            Search();

            base.OnInitialized();
        }

        protected override void Search()
        {
            CarBrands = new List<CarBrandView>();
            HideErrorMessage();

            var result = Service.Search(SearchView);

            result.Match(right => {

                SearchResponse = right;
                CarBrands = right.Content;

            }, ShowErrorMessage); 
        }
        
        protected override void DisplayPage(int pageNumber)
        {
            SearchView.Page = pageNumber;
            Search();
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

        protected void GetBrand(string id)
        {
            var holder = Service.FindById(id);

            holder.Match(ShowEditModal,
                () => ShowErrorMessage($"No se encontro informacion del fabricante con id: {id}"));
        }

        protected void GetLines(string id, string name)
        {
            NavigationManager.NavigateTo($"/car-brands/{id}/lines?CarBrandName={name}");
        }

        protected override void Update()
        {
            var result = Service.Update(CarBrandView);

            result.Match(right => {

                HideAddModal();
                Search();
            }, DisplayModalError);
        }

        protected override void Add()
        {
            var result = Service.Add(CarBrandView);
            
            result.Match(right => {

                HideAddModal();
                Search();
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