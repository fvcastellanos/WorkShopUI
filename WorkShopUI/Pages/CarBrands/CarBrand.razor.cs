
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class CarBrandBase: CrudBase
    {
        [Inject]
        protected CarBrandService Service { get; set; }

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
            // System.Console.WriteLine(pageNumber - 1);
            SearchView.Page = pageNumber - 1;
            Search();
        }
        protected void ShowAddModal()
        {
            CarBrandView = new CarBrandView()
            {
                Active = "Yes"
            };

            EditContext = new EditContext(CarBrandView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected void GetBrand(string id)
        {
            System.Console.WriteLine(id);
        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }

        protected override void Add()
        {
            var result = Service.Add(CarBrandView);

            result.Match(right => {

                HideAddModal();
                Search();
            }, DisplayModalError);
        }
    }
}