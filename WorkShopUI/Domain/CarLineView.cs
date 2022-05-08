using System.ComponentModel.DataAnnotations;

namespace WorkShopUI.Domain
{
    public class CarLineView
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