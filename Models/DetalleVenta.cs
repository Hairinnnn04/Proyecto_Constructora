using System.ComponentModel.DataAnnotations.Schema;
namespace miprimerproyecto.Models
{
    [Table("detalle_proyectos")]
    public class DetalleProyecto
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("proyecto_id")]
        public int ProyectoId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("material_id")]
        public int MaterialId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("cantidad")]
        public int Cantidad { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("costo_unitario")]
        public decimal CostoUnitario { get; set; }
    }
}
