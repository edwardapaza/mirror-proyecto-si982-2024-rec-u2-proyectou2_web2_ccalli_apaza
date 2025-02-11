using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animalia.Models
{
    public class Consulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConsulta { get; set; }

        [Required]
        public int IdMascota { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdVeterinario { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(10)]
        public string Hora { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        // Relaciones
        [ForeignKey("IdMascota")]
        public Mascota Mascota { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("IdVeterinario")]
        public Veterinario Veterinario { get; set; }

        public virtual Diagnostico Diagnostico { get; set; }
    }
}