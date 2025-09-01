using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miprimerproyecto.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Column("rol_id")]
    public int RolId { get; set; }
    // public string Rol { get; set; } = string.Empty;
    }
}
