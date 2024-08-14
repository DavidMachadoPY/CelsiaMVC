namespace Celsia.ViewModels
{
    // Para mostrar información de una transacción (ejemplo: en un historial de transacciones)
    public class TransactionViewModel
    {
        public string? Id { get; set; }
        public DateTime? DateTime { get; set; }
        public decimal? Amount { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? UserName { get; set; }
        public string? PlatformName { get; set; }
    }
}
