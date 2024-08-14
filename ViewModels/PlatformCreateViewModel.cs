using System.ComponentModel.DataAnnotations;

namespace Celsia.ViewModels
{
    // Para crear una nueva plataforma
    public class PlatformCreateViewModel
    {
        [Required(ErrorMessage = "El nombre de la plataforma es obligatorio")]
        public string? Name { get; set; }
    }
}
