namespace Celsia.Models
{
    public class Platform
    {
        public int Id { get; set; }  // Identificador único de la plataforma
        public string? Name { get; set; }  // Nombre de la plataforma

        // Relación uno a muchos: Una plataforma puede ser utilizada en muchas transacciones
        public ICollection<Transaction>? Transactions { get; set; }
    }
}