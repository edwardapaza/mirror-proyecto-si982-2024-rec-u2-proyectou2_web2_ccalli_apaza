using Microsoft.AspNetCore.Mvc;
using Animalia.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Animalia.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var consultas = _context.vw_ConsultasNotificaciones.ToList();
            return View(consultas);
        }

        [HttpPost]
        public IActionResult SendNotification(int idConsulta)
        {
            try
            {
                var consulta = _context.vw_ConsultasNotificaciones
                                         .Where(c => c.IdConsulta == idConsulta)
                                         .FirstOrDefault();

                if (consulta == null)
                {
                    TempData["ErrorMessage"] = "Consulta no encontrada.";
                    return RedirectToAction("Index");
                }

                if (consulta.CanSendNotification.Equals(""))
                {
                    TempData["ErrorMessage"] = "No se puede enviar la notificación para consultas pasadas.";
                    return RedirectToAction("Index");
                }

                using (var connection = _context.Database.GetDbConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "sp_EnviarRecordatorioConsulta";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@IdConsulta", idConsulta));
                        command.ExecuteNonQuery();
                    }
                }

                TempData["SuccessMessage"] = "Notificación enviada correctamente.";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al enviar la notificación. Por favor, revise el registro de errores: " + ex.Message;
                Console.WriteLine($"Error al enviar notificación: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}