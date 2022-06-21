using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain.Views;

namespace WorkShopUI.Transformers
{
    public class ProductTransformer : BaseTransformer
    {
        public static ProductView ToView(Product product)
        {
            return new ProductView
            {
                Id = GetId(product),
                Name = product.Name,
                Code = product.Code,
                Description = product.Description,
                Type = product.Type ,
                Active = product.Active,
            };
        }

        public static Product ToModel(ProductView productView)
        {
            return new Product
            {
                Code = productView.Code,
                Type = productView.Type,
                Name = productView.Name,
                Description = productView.Description,
                Active = productView.Active
            };
        }
    }
}