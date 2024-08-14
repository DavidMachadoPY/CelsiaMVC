using System.ComponentModel.DataAnnotations;

namespace Celsia.ViewModels
{
    // Para actualizar la información de una plataforma
    public class PlatformUpdateViewModel
    {
        [Required(ErrorMessage = "El nombre de la plataforma es obligatorio")]
        public string? Name { get; set; }
    }
}
