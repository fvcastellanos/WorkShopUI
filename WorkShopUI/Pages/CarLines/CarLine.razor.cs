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

        protected SearchView SearchView;
        protected IEnumerable<CarLineView> CarLines;
        protected CarLineView CarLineView;

        protected override async Task OnInitializedAsync()
        {
            SearchView = new SearchView
            {
                Page = 0,
                Size = 25,
                Active = "ACTIVE",
                Name = ""
            };

            HideAddModal();
            await SearchAsync();
        }

        protected override async Task SearchAsync()
        {
            CarLines = new List<CarLineView>();
            HideErrorMessage();

            var result = await Service.SearchAsync(CarBrandId, SearchView);

            result.Match(right => {

                CarLines = right;

            }, ShowErrorMessage);

        }

        protected override async Task AddAsync()
        {
            var result = await Service.AddAsync(CarLineView);

            result.Match(async right => {

                HideAddModal();
                await SearchAsync();
            }, DisplayModalError);
        }

        protected override async Task UpdateAsync()
        {
            var result = await Service.UpdateAsync(CarLineView);

            result.Match(async right => {

                HideAddModal();
                await SearchAsync();
            }, DisplayModalError);
        }

        protected async Task GetCarLineAsync(string id)
        {
            var holder = await Service.FindByIdAsync(id);

            holder.Match(ShowEditModal, 
                () => ShowErrorMessage("No se pudo obtener la línea de vehículo"));
        }

        protected void ShowAddModal()
        {
            CarLineView = new CarLineView()
            {
                CarBrandId = CarBrandId,
                Active = "ACTIVE"
            };

            EditContext = new EditContext(CarLineView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected override async Task DisplayPageAsync(int pageNumber)
        {
            SearchView.Page = pageNumber;
            await SearchAsync();
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