using System;

namespace miprimerproyecto.Models
{
    public class Proyecto
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("usuario_id")]
        public int UsuarioId { get; set; }
        // public string Responsable { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.Schema.Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("fecha_fin")]
    public DateTime FechaFin { get; set; }
        public decimal Presupuesto { get; set; }
    }
}
