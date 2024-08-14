namespace Celsia.Models
{
    public class Transaction
    {
        public string? Id { get; set; }  // Identificador único de la transacción
        public DateTime DateTime { get; set; }  // Fecha y hora de la transacción
        public decimal Amount { get; set; }  // Monto de la transacción
        public string? Status { get; set; }  // Estado de la transacción (pendiente, completada, fallida, etc.)
        public string? Type { get; set; }  // Tipo de transacción (pago, transferencia, etc.)

        public string? UserId { get; set; }  // Clave foránea para el usuario
        public User? User { get; set; }  // Relación con el usuario

        public int PlatformId { get; set; }  // Clave foránea para la plataforma
        public Platform? Platform { get; set; }  // Relación con la plataforma

        // Relación uno a muchos: Una transacción puede tener muchas facturas
        public ICollection<Invoice>? Invoices { get; set; }
    }
}