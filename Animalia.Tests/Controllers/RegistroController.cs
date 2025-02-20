using Microsoft.AspNetCore.Mvc;
using Animalia.Data;
using Animalia.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;


namespace Animalia.Controllers
{
    public class RegistroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistroController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Clientes = _context.Clientes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RegistrarMascota(Mascota mascota)
        {
            _context.Mascotas.Add(mascota);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Buscar(string tipo, string criterio)
        {
            if (tipo == "cliente")
            {
                var clientes = _context.Clientes
                    .Where(c => c.Nombre.Contains(criterio) || c.Dni.Contains(criterio))
                    .Select(c => new
                    {
                        Nombre = c.Nombre,
                        DNI = c.Dni,
                        Email = c.Email,
                        Dirección = c.Direccion,
                        Teléfono = c.Telefono
                    })
                    .ToList();
                return Json(clientes);
            }
            else if (tipo == "mascota")
            {
                var mascotas = _context.Mascotas
                    .Where(m => m.Nombre.Contains(criterio) || m.Especie.Contains(criterio) || m.Color.Contains(criterio))
                    .Select(m => new
                    {
                        Nombre = m.Nombre,
                        Especie = m.Especie,
                        Raza = m.Raza,
                        Edad = m.Edad,
                        Color = m.Color,
                        Dueño = _context.Clientes.Where(c => c.IdCliente == m.IdCliente)
                                                 .Select(c => c.Nombre)
                                                 .FirstOrDefault() ?? "No registrado"
                    })
                    .ToList();
                return Json(mascotas);
            }
            return Json(new { mensaje = "No se encontraron resultados" });
        }

    }
}
