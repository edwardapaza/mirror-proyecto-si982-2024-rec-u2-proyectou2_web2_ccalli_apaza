using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animalia.Models
{
    [Table("vw_ConsultasNotificaciones")]
    public class vw_ConsultasNotificaciones
    {
        [Key]
        public int IdConsulta { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Descripcion { get; set; }
        public string NombreMascota { get; set; }
        public string NombreCliente { get; set; }
        public string NombreVeterinario { get; set; }
        public int IdCliente { get; set; }
        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }
        public int CanSendNotification { get; set; }
    }
}