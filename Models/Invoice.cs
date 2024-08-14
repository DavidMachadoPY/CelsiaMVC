using System.ComponentModel.DataAnnotations;

namespace Celsia.Models
{
    public class Invoice
    {
        [Key]
        public string? Number { get; set; }  // Identificador único de la factura
        public string? TransactionId { get; set; }  // Clave foránea para la transacción
        public Transaction? Transaction { get; set; }  // Relación con la transacción
        public string? Period { get; set; }  // Periodo de facturación
        public decimal BilledAmount { get; set; }  // Monto facturado
        public decimal PaidAmount { get; set; }  // Monto pagado
    }
}