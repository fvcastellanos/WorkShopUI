using LanguageExt;
using WorkShopUI.Clients;
using WorkShopUI.Domain.Views;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class WorkOrderService
    {
        private readonly ILogger _logger;

        private readonly WorkOrderClient _workOrderClient;

        public WorkOrderService(ILogger<WorkOrderService> logger, WorkOrderClient workOrderClient)
        {
            _logger = logger;
            _workOrderClient = workOrderClient;
        }

        public Either<string, PagedView<WorkOrderView>> Search(WorkOrderSearchView searchView)
        {
            try
            {
                var searchResult = _workOrderClient.Search(searchView.Text,searchView.Status, 
                    searchView.Page, searchView.Size);

                return new PagedView<WorkOrderView>
                {
                    Pageable = BaseTransformer.BuildPageable(searchResult),
                    Content = searchResult.Content
                        .Select(WorkOrderTransformer.ToView)
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to search work orders - ");
                return "No es posible buscar ordenes de trabajo";
            }
        }

        public Option<WorkOrderView> FindById(string id)
        {
            try
            {
                return _workOrderClient.FindById(id)
                    .Map(WorkOrderTransformer.ToView);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to find work order with id: {id}");
                return null;
            }
        }

    }
}