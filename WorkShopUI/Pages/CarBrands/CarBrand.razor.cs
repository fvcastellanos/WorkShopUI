
using Microsoft.AspNetCore.Components;
using WorkShopUI.Domain;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class CarBrandBase: CrudBase
    {
        [Inject]
        protected CarBrandService Service { get; set; }
        protected IEnumerable<CarBrandView> CarBrands;
        protected SearchView SearchView;

        protected override void OnInitialized()
        {
            SearchView = new SearchView
            {
                Page = 1,
                Size = 20,
                Active = 1,
                Name = ""
            };

            HideAddModal();
            Search();
        }

        protected void Search()
        {
            CarBrands = new List<CarBrandView>();
            HideErrorMessage();

            var result = Service.Search(SearchView);
            result.Match(right => CarBrands = right, ShowErrorMessage); 
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