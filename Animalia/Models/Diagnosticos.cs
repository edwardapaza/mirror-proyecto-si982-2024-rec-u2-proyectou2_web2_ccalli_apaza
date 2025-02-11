using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animalia.Models
{
    public class Diagnostico
    {
        [Key]
        public int IdDiagnostico { get; set; }

        [Required]
        public int IdConsulta { get; set; }

        [ForeignKey("IdConsulta")]
        public Consulta Consulta { get; set; }

        [Required]
        public decimal Peso { get; set; }

        [MaxLength(500)]
        public string Observaciones { get; set; }

        [MaxLength(500)]
        public string ExamenesRealizados { get; set; }

        [Required]
        [MaxLength(1000)]
        public string DiagnosticoGeneral { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}