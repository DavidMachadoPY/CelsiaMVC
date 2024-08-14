using System.ComponentModel.DataAnnotations;

namespace Celsia.ViewModels
{
    // Para crear una nueva factura
    public class InvoiceCreateViewModel
    {
        [Required(ErrorMessage = "El ID de la transacción es obligatorio")]
        public string? TransactionId { get; set; }

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
