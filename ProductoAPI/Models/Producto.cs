using System.ComponentModel.DataAnnotations;

namespace ProductoAPI.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string? Categoria { get; set; }

        public string? Descripcion { get; set; }
    }
}
