using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Animalia.Models;
using System.Linq;
using System.Threading.Tasks;
using Animalia.Data;

namespace Animalia.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var productos = from p in _context.Productos
                            select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString) || p.Categoria.Contains(searchString));
            }

            // No longer using ViewBag, returning View with model directly
            ViewData["CurrentFilter"] = searchString; // to keep search string in input after postback
            return View(await productos.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            _context.Add(producto);
            await _context.SaveChangesAsync();
            // ViewBag.Productos = await _context.Productos.ToListAsync(); // No longer using ViewBag
            return View("Index", await _context.Productos.ToListAsync()); // Return to Index view with updated list
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(); // Handle case where product is not found
            }
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return View("Index", await _context.Productos.ToListAsync());
        }

    }
}