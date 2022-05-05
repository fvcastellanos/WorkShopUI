using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;

namespace WorkShopUI.Transformers
{
    public class CarBrandTransformer : BaseTransformer
    {
        public static CarBrand ToModel(CarBrandView carBrandView)
        {
            return new CarBrand
            {
                Name = carBrandView.Name,
                Description = carBrandView.Description,
                Active = carBrandView.Active.Equals("Yes") ? "ACTIVE" : "INACTIVE"
            };
        }

        public static CarBrandView ToView(CarBrand carBrand)
        {
            return new CarBrandView
            {
                Id = GetId(carBrand.Links),
                Name = carBrand.Name,
                Description = carBrand.Description,
                Active = carBrand.Active
            };
        }
    }
}