
using Microsoft.AspNetCore.Components;
using WorkShopUI.Domain;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class CarBrandBase: CrudBase
    {
        [Inject]
        protected CarBrandService Service { get; set; }

        protected PagedView<CarBrandView> SearchResponse;
        protected IEnumerable<CarBrandView> CarBrands;
        protected SearchView SearchView;

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
        
        protected void DisplayPage(int pageNumber)
        {
            SearchView.Page = pageNumber - 1;
            Search();
        }
        protected void ShowAddModal()
        {

        }

        protected void AddBrand()
        {

        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }

        protected override void Add()
        {
            throw new NotImplementedException();
        }
    }
}