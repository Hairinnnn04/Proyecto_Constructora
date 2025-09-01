using System.ComponentModel.DataAnnotations;

namespace miprimerproyecto.Models
{
    public class Material
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
