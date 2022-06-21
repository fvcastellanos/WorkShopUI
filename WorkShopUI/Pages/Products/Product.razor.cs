using Microsoft.AspNetCore.Components;
using WorkShopUI.Domain.Views;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class ProductBase : CrudBase
    {
        [Inject]
        protected ProductService ProductService { get; set; }
        protected SearchView? SearchView;

        protected PagedView<ProductView>? SearchResponse;

        protected IEnumerable<ProductView>? Products;

        protected ProductView? ProductView;

        protected override Task OnInitializedAsync()
        {
            SearchView = new SearchView
            {
                Active = 1,
                Code = "",
                Name = "",
                Type = "P",
                Page = 0,
                Size = 25
            };

            Search();

            return base.OnInitializedAsync();
        }
        protected override void Add()
        {
            throw new NotImplementedException();
        }

        protected override void DisplayPage(int pageNumber)
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            Products = new List<ProductView>();

            HideErrorMessage();
            
            var result = ProductService.Search(SearchView);

            result.Match(right => {

                SearchResponse = right;
                Products = right.Content;
            }, ShowErrorMessage);
        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }

        protected void ShowAddModal()
        {

        }

        protected void GetProduct(string id)
        {

        }
    }
}