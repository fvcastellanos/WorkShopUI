using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShopUI.Domain.Views;
using WorkShopUI.Services;

namespace WorkShopUI.Pages
{
    public class WorkOrderBase : CrudBase
    {
        [Inject]
        protected WorkOrderService WorkOrderService { get; set; }
        
        [Inject]
        protected CarLineService CarLineService { get; set; }
        
        [Inject]
        protected ContactService ContactService { get; set; }
        
        protected WorkOrderSearchView SearchView;

        protected PagedView<WorkOrderView> SearchResponse;

        protected IEnumerable<WorkOrderView> WorkOrders;

        protected string CarLineTextSearch;
        protected IEnumerable<CarLineView> Carlines;
        protected CarLineView SelectedCarLine;

        protected string ContactTextSearch;
        protected IEnumerable<ContactView> Contacts;
        protected ContactView SelectedContact;

        protected WorkOrderView WorkOrderView;

        protected DateTime OrderDate;
        
        protected override void OnInitialized()
        {
            SearchView = new WorkOrderSearchView
            {
                Text = "",
                Status = "%",
                Page = 0,
                Size = 25
            };

            WorkOrderView = new WorkOrderView();
            HideAddModal();
            Search();
            
            base.OnInitialized();
        }


        protected void ShowAddModal()
        {
            WorkOrderView = new WorkOrderView
            {
                OdometerMeasurement = "K",
                Status = "IN_PROGRESS",
            };

            OrderDate = DateTime.Now;

            Carlines = new List<CarLineView>();
            CarLineTextSearch = "";
            ContactTextSearch = "";

            EditContext = new EditContext(WorkOrderView);
            ModifyModal = false;
            
            HideErrorMessage();
            ShowModal();
        }

        protected override void Add()
        {
            // throw new NotImplementedException();
            
            HideAddModal();
        }

        protected override void Search()
        {
            WorkOrders = new List<WorkOrderView>();
            HideErrorMessage();
            
            var result = WorkOrderService.Search(SearchView);

            result.Match(right =>
            {
                SearchResponse = right;
                WorkOrders = right.Content;
            }, ShowErrorMessage);
        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }
        
        protected void OnReadData(DataGridReadDataEventArgs<WorkOrderView> eventArgs)
        {
            SearchView.Page = eventArgs.Page - 1;
            Search();
        }

        protected void GetWorkOrder(string id)
        {
            var workOrderHolder = WorkOrderService.FindById(id);

            workOrderHolder.Match(ShowEditModal,
                () => ShowErrorMessage($"No se encontró información de la orden de trabajo id: {id}"));
        }

        protected void AutoCompleteCarLines(string searchText)
        {
            var carLineSearchView = new SearchView
            {
                Text = searchText,
                Active = 1,
                Page = 0,
                Size = 50
            };

            var result = CarLineService.SearchLines(carLineSearchView);
            result.Match(right => Carlines = right.Content, DisplayModalError);
        }

        protected void AutoCompleteContacts(string searchText)
        {
            var searchView = new ContactSearchView
            {
                Text = searchText,
                Type = "%",
                Active = 1,
                Page = 0,
                Size = 50
            };

            var result = ContactService.Search(searchView);
            result.Match(right => Contacts = right.Content, DisplayModalError);
        }

        protected void SaveChanges()
        {
            HideModalError();

            if (SelectedCarLine == null) 
            {
                DisplayModalError("Debe seleccionar una línea de vehículo");
                return;
            }

            if (SelectedContact == null)
            {
                DisplayModalError("Debe seleccionar un Contacto");
                return;
            }

            WorkOrderView.CarLineView = SelectedCarLine;
            WorkOrderView.ContactView = SelectedContact;
            WorkOrderView.OrderDate = OrderDate.ToShortDateString();

            base.SaveChanges();
        }

        private void ShowEditModal(WorkOrderView workOrderView)
        {
            WorkOrderView = workOrderView;
            EditContext = new EditContext(WorkOrderView);
        }
    }
}