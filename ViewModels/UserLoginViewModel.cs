using System.ComponentModel.DataAnnotations;

namespace Celsia.ViewModels
{
    // Para manejar el inicio de sesión del usuario
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string? Password { get; set; }  // Contraseña del usuario
    }
}
