using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Transformers
{
    public class WorkOrderTransformer : BaseTransformer
    {
        public static WorkOrderView ToView(WorkOrder workOrder)
        {
            return new WorkOrderView
            {
                Id = GetId(workOrder),
                Number = workOrder.Number,
                Status = workOrder.Status,
                OrderDate = workOrder.OrderDate,
                PlateNumber = workOrder.PlateNumber,
                OdometerMeasurement = workOrder.OdometerMeasurement,
                OdometerValue = workOrder.OdometerValue,
                GasAmount = int.Parse(workOrder.GasAmount.ToString()),
                Notes = workOrder.Notes,
                ContactView = ContactTransformer.ToView(workOrder.Contact),
                CarLineView = CarLineTransformer.ToView(workOrder.CarLine)
            };
        }
    }
}