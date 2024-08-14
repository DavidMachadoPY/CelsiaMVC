using System.ComponentModel.DataAnnotations;

namespace Celsia.ViewModels
{
    // Para actualizar la información de una factura
    public class InvoiceUpdateViewModel
    {
        [Required(ErrorMessage = "El periodo de facturación es obligatorio")]
        public string? Period { get; set; }

        [Required(ErrorMessage = "El monto facturado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto facturado debe ser mayor que cero")]
        public decimal? BilledAmount { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor que cero")]
        public decimal? PaidAmount { get; set; }
    }
}
