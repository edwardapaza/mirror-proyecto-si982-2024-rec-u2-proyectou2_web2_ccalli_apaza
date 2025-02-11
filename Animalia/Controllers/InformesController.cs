using Animalia.Data;
using Animalia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Animalia.Controllers
{
    public class InformesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var consultasConDiagnosticos = await _context.Consultas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m.Cliente)
                .Include(c => c.Diagnostico)
                .ToListAsync();

            return View(consultasConDiagnosticos);
        }

        public async Task<IActionResult> CreateDiagnostico(int idConsulta)
        {
            var consulta = await _context.Consultas.FindAsync(idConsulta);
            if (consulta == null)
            {
                return NotFound();
            }

            ViewBag.IdConsulta = idConsulta;
            return View("CreateDiagnostico");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiagnostico(Diagnostico diagnostico)
        {
            if (ModelState.IsValid)
            {
                _context.Diagnosticos.Add(diagnostico);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Informes");
            }
            return View("CreateDiagnostico", diagnostico);
        }

        public async Task<IActionResult> GenerarPdfDiagnostico(int idConsulta)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m.Cliente)
                .Include(c => c.Diagnostico)
                .FirstOrDefaultAsync(c => c.IdConsulta == idConsulta);

            if (consulta == null || consulta.Diagnostico == null)
            {
                return NotFound();
            }

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Verdana", 18, XFontStyleEx.Bold);
            XFont headingFont = new XFont("Verdana", 14, XFontStyleEx.Italic);
            XFont normalFont = new XFont("Verdana", 12, XFontStyleEx.Regular);
            XColor primaryColor = XColors.DarkSlateGray;
            XColor secondaryColor = XColors.Gray;

            double yPosition = 20;
            double xPosition = 20;
            double lineSpace = 20;

            void DrawFormattedText(string text, XFont font, XColor color)
            {
                gfx.DrawString(text, font, new XSolidBrush(color), new XRect(xPosition, yPosition, page.Width - 40, 30), XStringFormats.TopLeft);
                yPosition += lineSpace;
            }

            DrawFormattedText("Informe de Diagnóstico Veterinario", titleFont, primaryColor);
            yPosition += lineSpace;

            DrawFormattedText("Información de la Consulta", headingFont, primaryColor);
            DrawFormattedText($"ID Consulta: {consulta.IdConsulta}", normalFont, secondaryColor);
            DrawFormattedText($"Fecha: {consulta.Fecha.ToShortDateString()}", normalFont, secondaryColor);
            DrawFormattedText($"Hora: {consulta.Hora}", normalFont, secondaryColor);
            yPosition += lineSpace;

            DrawFormattedText("Información del Cliente", headingFont, primaryColor);
            DrawFormattedText($"Cliente: {consulta.Mascota.Cliente.Nombre}", normalFont, secondaryColor);
            yPosition += lineSpace;

            DrawFormattedText("Información de la Mascota", headingFont, primaryColor);
            DrawFormattedText($"Nombre: {consulta.Mascota.Nombre}", normalFont, secondaryColor);
            DrawFormattedText($"Especie: {consulta.Mascota.Especie}", normalFont, secondaryColor);
            DrawFormattedText($"Raza: {consulta.Mascota.Raza}", normalFont, secondaryColor);
            DrawFormattedText($"Edad: {consulta.Mascota.Edad} años", normalFont, secondaryColor);
            DrawFormattedText($"Color: {consulta.Mascota.Color}", normalFont, secondaryColor);
            yPosition += lineSpace;

            DrawFormattedText("Diagnóstico Veterinario", headingFont, primaryColor);
            DrawFormattedText($"Peso: {consulta.Diagnostico.Peso} kg", normalFont, secondaryColor);
            DrawFormattedText($"Observaciones: {consulta.Diagnostico.Observaciones}", normalFont, secondaryColor);
            DrawFormattedText($"Exámenes Realizados: {consulta.Diagnostico.ExamenesRealizados}", normalFont, secondaryColor);
            DrawFormattedText($"Diagnóstico General: {consulta.Diagnostico.DiagnosticoGeneral}", normalFont, secondaryColor);
            DrawFormattedText($"Fecha de Creación del Diagnóstico: {consulta.Diagnostico.FechaCreacion.ToString(CultureInfo.CurrentCulture)}", normalFont, secondaryColor);


            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            byte[] pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", $"Diagnostico_{consulta.IdConsulta}.pdf");
        }
    }
}