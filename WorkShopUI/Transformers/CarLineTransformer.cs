using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Transformers
{
    public class CarLineTransformer : BaseTransformer
    {
        public static CarLineView ToView(CarLine carLine)
        {
            return new CarLineView
            {
                Id = GetId(carLine),
                Name = carLine.Name,
                Description = carLine.Description,
                Active = carLine.Active
            };
        }

        public static CarLine ToModel(CarLineView view)
        {
            return new CarLine
            {
                Name = view.Name,
                Description = view.Description,
                Active = view.Active
            };
        }
    }
}