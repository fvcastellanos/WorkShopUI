using System.ComponentModel.DataAnnotations;

namespace WorkShopUI.Domain.Views
{
    public class ProductView
    {
        public string? Id { get; set; }

        [Required]
        [EnumDataType(typeof(ProductTypeView))]
        public string? Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Code { get; set; }

        [Required]
        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public double? MinimalQuantity { get; set; }

        public double? SalePrice { get; set; }

        [EnumDataType(typeof(ActiveTypeView))]
        public string? Active { get; set; }
    }
}