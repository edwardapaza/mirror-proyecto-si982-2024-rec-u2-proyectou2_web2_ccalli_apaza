using Animalia.Data;
using Animalia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using iText.IO.Font;

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

            // Configuración del PDF con iText 7
            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Rutas a tus fuentes desde wwwroot/Assets
                string helveticaBoldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Helvetica-Bold.ttf");
                string helveticaBoldObliquePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Helvetica-BoldOblique.ttf");
                string helveticaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Helvetica.ttf");


                // Fuentes y colores -  Cargando las fuentes desde archivos .ttf
                PdfFont titleFont = PdfFontFactory.CreateFont(helveticaBoldPath, PdfEncodings.WINANSI);
                PdfFont headingFont = PdfFontFactory.CreateFont(helveticaBoldObliquePath, PdfEncodings.WINANSI);
                PdfFont normalFont = PdfFontFactory.CreateFont(helveticaPath, PdfEncodings.WINANSI);
                Color primaryColor = new DeviceRgb(47, 79, 79);
                Color secondaryColor = new DeviceRgb(128, 128, 128);

                // Título
                document.Add(new Paragraph("Informe de Diagnóstico Veterinario")
                    .SetFont(titleFont)
                    .SetFontSize(18)
                    .SetFontColor(primaryColor)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                // Información de la Consulta
                document.Add(new Paragraph("Información de la Consulta")
                    .SetFont(headingFont)
                    .SetFontSize(14)
                    .SetFontColor(primaryColor)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"ID Consulta: {consulta.IdConsulta}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Fecha: {consulta.Fecha.ToShortDateString()}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Hora: {consulta.Hora}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor))
                    .SetBottomMargin(20);

                // Información del Cliente
                document.Add(new Paragraph("Información del Cliente")
                    .SetFont(headingFont)
                    .SetFontSize(14)
                    .SetFontColor(primaryColor)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Cliente: {consulta.Mascota.Cliente.Nombre}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor))
                    .SetBottomMargin(20);

                // Información de la Mascota
                document.Add(new Paragraph("Información de la Mascota")
                    .SetFont(headingFont)
                    .SetFontSize(14)
                    .SetFontColor(primaryColor)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Nombre: {consulta.Mascota.Nombre}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Especie: {consulta.Mascota.Especie}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Raza: {consulta.Mascota.Raza}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Edad: {consulta.Mascota.Edad} años")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Color: {consulta.Mascota.Color}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor))
                    .SetBottomMargin(20);

                document.Add(new Paragraph("Diagnóstico Veterinario")
                    .SetFont(headingFont)
                    .SetFontSize(14)
                    .SetFontColor(primaryColor)
                    .SetMarginBottom(10));

                document.Add(new Paragraph($"Peso: {consulta.Diagnostico.Peso} kg")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Observaciones: {consulta.Diagnostico.Observaciones}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Exámenes Realizados: {consulta.Diagnostico.ExamenesRealizados}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Diagnóstico General: {consulta.Diagnostico.DiagnosticoGeneral}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));
                document.Add(new Paragraph($"Fecha de Creación del Diagnóstico: {consulta.Diagnostico.FechaCreacion.ToString(CultureInfo.CurrentCulture)}")
                    .SetFont(normalFont)
                    .SetFontSize(12)
                    .SetFontColor(secondaryColor));

                document.Close();

                byte[] pdfBytes = stream.ToArray();
                return File(pdfBytes, "application/pdf", $"Diagnostico_{consulta.IdConsulta}.pdf");
            }
        }
    }
}