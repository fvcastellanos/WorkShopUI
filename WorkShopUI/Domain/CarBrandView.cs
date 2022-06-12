
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkShopUI.Domain
{
    public class CarBrandView
    {
        public string Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [EnumDataType(typeof(ActiveTypeView))]
        public string Active { get; set; }
    }
}
