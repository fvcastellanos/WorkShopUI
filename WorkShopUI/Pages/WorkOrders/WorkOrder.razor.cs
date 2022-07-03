using WorkShopUI.Domain.Views;

namespace WorkShopUI.Pages
{
    public class WorkOrderBase : CrudBase
    {

        protected WorkOrderSearchView SearchView;

        protected IEnumerable<WorkOrderView> WorkOrders;

        protected override void OnInitialized()
        {
            SearchView = new WorkOrderSearchView
            {
                Number = "",
                PlateNumber = "",
                Status = "P",
                Page = 0,
                Size = 25
            };

            base.OnInitialized();
        }


        protected void ShowAddModal() 
        {

        }

        protected override void Add()
        {
            throw new NotImplementedException();
        }

        protected override void Search()
        {
            throw new NotImplementedException();
        }

        protected override void Update()
        {
            throw new NotImplementedException();
        }
    }
}