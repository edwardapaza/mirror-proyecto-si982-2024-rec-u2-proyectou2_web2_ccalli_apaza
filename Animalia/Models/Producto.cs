using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animalia.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}