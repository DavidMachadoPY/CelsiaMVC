namespace Celsia.ViewModels
{
    // Para crear un nuevo usuario
    public class UserCreateViewModel
    {
        public string? Id { get; set; }  // Numero de Identificador
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }  // Contraseña del usuario
    }
}