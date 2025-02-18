using Microsoft.AspNetCore.Mvc;
using Animalia.Data;
using Animalia.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Animalia.Controllers
{
    public class AgendamientoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendamientoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var consultas = _context.Consultas
                .Include(c => c.Mascota)
                .ThenInclude(m => m.Cliente)
                .Include(c => c.Veterinario)
                .ToList();

            ViewBag.Mascotas = _context.Mascotas.Include(m => m.Cliente).ToList();
            ViewBag.Veterinarios = _context.Veterinarios.ToList();
            ViewBag.Consultas = consultas;

            return View();
        }

        [HttpPost]
        public IActionResult AgregarConsulta(Consulta consulta)
        {
            var mascota = _context.Mascotas.Include(m => m.Cliente).FirstOrDefault(m => m.IdMascota == consulta.IdMascota);
            if (mascota != null)
            {
                consulta.IdCliente = mascota.IdCliente;
                consulta.Descripcion ??= "Consulta médica agendada";

                _context.Consultas.Add(consulta);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            TempData["Error"] = "Error al seleccionar la mascota.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ReprogramarConsulta(int IdConsulta, string NuevaFecha, string NuevaHora)
        {
            var consulta = _context.Consultas.Find(IdConsulta);
            if (consulta == null)
            {
                TempData["Error"] = "Consulta no encontrada.";
                return RedirectToAction("Index");
            }

            consulta.Fecha = DateTime.Parse(NuevaFecha);
            consulta.Hora = NuevaHora;
            _context.Consultas.Update(consulta);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CancelarConsulta(int IdConsulta)
        {
            var consulta = _context.Consultas.Find(IdConsulta);
            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Consulta no encontrada.";
            return RedirectToAction("Index");
        }
    }
}
