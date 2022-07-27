using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

        protected override void OnInitialized()
        {
            SearchView = new SearchView
            {
                Active = 1,
                Text = "",
                Type = "%",
                Page = 0,
                Size = 25
            };

            ProductView = new ProductView();
            Search();
            base.OnInitialized();
        }

        protected override void Add()
        {
            var result = ProductService.Add(ProductView);

            result.Match(right => {

                HideAddModal();
                Search();

            }, DisplayModalError);
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
            var result = ProductService.Update(ProductView);

            result.Match(right => {

                HideAddModal();
                Search();
            }, DisplayModalError);
        }

        protected void ShowAddModal()
        {
            ProductView = new ProductView
            {
                Active = "ACTIVE",
                Type = "PRODUCT"
            };

            EditContext = new EditContext(ProductView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected void GetProduct(string id)
        {
            var holder = ProductService.FindById(id);

            holder.Match(ShowEditModal, 
                () => DisplayModalError("No se encuentra el producto") );
        }

        protected void OnReadData(DataGridReadDataEventArgs<ProductView> eventArgs)
        {
            SearchView.Page = eventArgs.Page - 1;
            Search();
        }

        // ------------------------------------------------------------------------------------------

        private void ShowEditModal(ProductView productView)
        {
            ProductView = productView;
            EditContext = new EditContext(ProductView);
            ShowEditModal();
        }
    }
}