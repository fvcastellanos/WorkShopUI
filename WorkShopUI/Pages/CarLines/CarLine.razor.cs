using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain.Views;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class CarLineBase : CrudBase
    {
        [Parameter]
        public string CarBrandId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "CarBrandName")]
        public string CarBrandName { get; set; }

        [Inject]
        protected CarLineService Service { get; set; }

        protected PagedView<CarLineView> SearchResponse;
        protected SearchView SearchView;
        protected IEnumerable<CarLineView> CarLines;
        protected CarLineView CarLineView;

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
            CarLines = new List<CarLineView>();
            HideErrorMessage();

            var result = Service.Search(CarBrandId, SearchView);

            result.Match(right => {

                SearchResponse = right;
                CarLines = right.Content;
            }, ShowErrorMessage);
        }

        protected override void Add()
        {
            var result = Service.Add(CarBrandId, CarLineView);

            result.Match(right => {

                HideAddModal();
                Search();
            }, DisplayModalError);
        }

        protected override void Update()
        {
            var result = Service.Update(CarBrandId, CarLineView);

            result.Match(right => {

                HideAddModal();
                Search();
            }, DisplayModalError);
        }

        protected void GetCarLine(string id)
        {
            var holder = Service.FindById(CarBrandId, id);

            holder.Match(ShowEditModal, 
                () => ShowErrorMessage("No se pudo obtener la línea de vehículo"));
        }

        protected void ShowAddModal()
        {
            CarLineView = new CarLineView()
            {
                Active = "ACTIVE"
            };

            EditContext = new EditContext(CarLineView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected override void DisplayPage(int pageNumber)
        {
            SearchView.Page = pageNumber;
            Search();
            StateHasChanged();
        }

        // ------------------------------------------------------------------------------------

        private void ShowEditModal(CarLineView view)
        {
            CarLineView = view;
            EditContext = new EditContext(CarLineView);

            ShowEditModal();
        }
    }
}