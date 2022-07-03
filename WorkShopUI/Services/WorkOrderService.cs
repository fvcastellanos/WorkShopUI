using WorkShopUI.Clients;

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

        
    }
}