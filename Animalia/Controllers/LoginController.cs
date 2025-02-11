using Microsoft.AspNetCore.Mvc;
using Animalia.Data;
using Animalia.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Animalia.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nombreUsuario, string contrasena)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            if (usuario == null || !VerifyPassword(contrasena, usuario.Contrasena))
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View("Index");
            }

            HttpContext.Session.SetString("Usuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", usuario.Rol);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                string pass = string.Concat(bytes.Select(b => b.ToString("x2")));
                Console.WriteLine(pass);
                return string.Concat(bytes.Select(b => b.ToString("x2")));
            }
        }

        private static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return HashPassword(inputPassword) == hashedPassword;
        }
    }
}
