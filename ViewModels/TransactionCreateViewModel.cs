using System;
using System.ComponentModel.DataAnnotations;

namespace Celsia.ViewModels
{
    // Para crear una nueva transacción
    public class TransactionCreateViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "La fecha y hora de la transacción son obligatorias")]
        public DateTime? DateTime { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "El estado de la transacción es obligatorio")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "El tipo de transacción es obligatorio")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "El ID de la plataforma es obligatorio")]
        public int? PlatformId { get; set; }
    }
}
