namespace Celsia.Models
{
    public class User
    {
        public string? Id { get; set; }  // Identificador único del usuario
        public string? Name { get; set; }  // Nombre del usuario
        public string? Address { get; set; }  // Dirección del usuario
        public string? Phone { get; set; }  // Teléfono del usuario
        public string? Email { get; set; }  // Correo electrónico del usuario
        public string? Password { get; set; }  // Contraseña del usuario, debería estar encriptada
        public string? Status { get; set; }  // Estado del usuario (por defecto, activo)
        public DateTime? CreateAt { get; set; } // Fecha de creación del registro
        public DateTime? UpdateAt { get; set; } // Fecha de última actualización del registro

        // Relación uno a muchos: Un usuario puede tener muchas transacciones
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
