using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;

namespace WorkShopUI.Transformers
{
    public class CarBrandTransformer : BaseTransformer
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