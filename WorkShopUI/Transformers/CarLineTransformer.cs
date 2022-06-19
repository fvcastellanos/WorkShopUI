using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;

namespace WorkShopUI.Transformers
{
    public class CarLineTransformer : BaseTransformer
    {
        public static CarLineView ToView(CarLine carLine)
        {
            return new CarLineView
            {
                Id = carLine.Id,
                CarBrandId = carLine.CarBrandId,
                Name = carLine.Name,
                Description = carLine.Description,
                Active = carLine.Active,
                Tenant = carLine.Tenant
            };
        }

        public static CarLine ToModel(CarLineView view)
        {
            return new CarLine
            {
                CarBrandId = view.CarBrandId,
                Name = view.Name,
                Description = view.Description,
                Active = view.Active
            };
        }
    }
}