using System.ComponentModel.DataAnnotations;

namespace WorkShopUI.Domain.Views
{
    public class WorkOrderView
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Valor requerido")]
        [MaxLength(50)]
        public string Number { get; set; }

        [Required(ErrorMessage = "Valor requerido")]
        public string OrderDate { get; set; }
        
        [Required]
        [EnumDataType(typeof(WorkOrderStatus))]
        public string Status { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string PlateNumber { get; set; }

        [Required]
        [MaxLength(1)]
        public string OdometerMeasurement { get; set; }

        public double OdometerValue { get; set; }

        public int GasAmount { get; set; }
        
        public string Notes { get; set; }
        
        public CarLineView CarLineView { get; set; }
        
        public ContactView ContactView { get; set; }
    }
}