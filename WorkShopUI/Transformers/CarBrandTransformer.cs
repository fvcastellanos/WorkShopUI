using WorkShopUI.Domain.Model;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Transformers
{
    public class CarBrandTransformer
    {
        public static CarBrandView ToView(CarBrand carBrand)
        {
            return new CarBrandView
            {
                Id = carBrand.Id,
                Name = carBrand.Name,
                Description = carBrand.Description,
                Active = carBrand.Active,
                Tenant = carBrand.Tenant
            };
        }

    }
}