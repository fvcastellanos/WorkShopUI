using System.ComponentModel.DataAnnotations;

namespace WorkShopUI.Domain.Views
{

    public class ContactView
    {
        public string Id { get; set; }

        [Required]
        [EnumDataType(typeof(ContactTypeView))]
        public string Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Contact { get; set; }

        [MaxLength(50)]
        public string TaxId { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }


        [EnumDataType(typeof(ActiveTypeView))]
        public string Active { get; set; }
    }
}