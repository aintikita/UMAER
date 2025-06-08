using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Diagnostics;
using FrontendMAUI.Models;
using System.Reflection;

namespace FrontendMAUI.Services;

// Servicio para generar un PDF con los datos clínicos de un paciente
public class PdfGeneratorService
{
    // Genera el PDF y lo devuelve como un array de bytes
    public async Task<byte[]> GenerarPdfAsync(
        Paciente paciente,
        List<Constante> constantes,
        List<Medicacion> medicaciones,
        List<Diuresis> diuresis,
        List<BalanceHidrico> balances,
        List<Alergia> alergias,
        List<CuidadoEnfermeria> cuidados,
        string nombreFirmante,
        Stream? graficoStream = null)
    {
        var documento = new PdfDocument();
        var pagina = documento.AddPage();
        var gfx = XGraphics.FromPdfPage(pagina);

        var fuenteTitulo = new XFont("Arial", 18, XFontStyle.Bold);
        var fuenteSeccion = new XFont("Arial", 14, XFontStyle.Bold);
        var fuenteTexto = new XFont("Arial", 12);

        double y = 40;

        // Logo en la parte superior
        AñadirLogoEnCadaPagina(gfx, pagina);
        y += 100;

        // Título principal
        gfx.DrawString("Informe de Control de Enfermería", fuenteTitulo, XBrushes.Black, new XRect(40, y, pagina.Width - 80, 30), XStringFormats.TopLeft);
        y += 60;

        gfx.DrawLine(XPens.Black, 40, y, pagina.Width - 40, y);
        y += 15;

        // Datos básicos del paciente
        gfx.DrawString("Datos del paciente", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 25;
        gfx.DrawString($"Nombre: {paciente.Nombre}", fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 20;
        gfx.DrawString($"Edad: {paciente.Edad} años", fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 20;
        gfx.DrawString($"Peso: {paciente.Peso} kg", fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 20;
        gfx.DrawString($"Altura: {paciente.Altura} cm", fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 20;
        gfx.DrawString($"Habitación: {paciente.Habitacion}", fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 20;
        gfx.DrawString($"Unidad: {paciente.Unidad}", fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 30;

        // Sección de alergias
        gfx.DrawString("Alergias", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 25;
        if (alergias.Any())
        {
            foreach (var a in alergias)
            {
                gfx.DrawString($"• {a.alergia}: {a.descripcion}", fuenteTexto, XBrushes.Black, new XPoint(50, y));
                y += 18;
            }
        }
        else
        {
            gfx.DrawString("No se han registrado alergias.", fuenteTexto, XBrushes.Black, new XPoint(50, y));
            y += 18;
        }

        y += 30;

        // Si hay gráfico, lo añade y pasa de página
        if (graficoStream != null)
        {
            gfx.DrawString("Gráfica de constantes", fuenteSeccion, XBrushes.Black, new XPoint(40, y));
            y += 30;

            var chart = XImage.FromStream(() => graficoStream);
            gfx.DrawImage(chart, 40, y, 500, 250);
            y += 270;

            // Salto de página tras la gráfica
            pagina = documento.AddPage();
            gfx = XGraphics.FromPdfPage(pagina);
            y = 40;
            AñadirLogoEnCadaPagina(gfx, pagina);
            y += 100;
        }

        // Constantes registradas
        gfx.DrawString("Constantes registradas", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 25;
        foreach (var c in constantes)
        {
            string linea = $"{c.fechaHora:dd/MM HH:mm} - Tº {c.temperatura}°C, FC {c.frecuenciaCardiaca}, FR {c.frecuenciaRespiratoria}, PA {c.presionArterial}, Sat {c.saturacionOxigeno}%";
            gfx.DrawString(linea, fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 18;

            // Salto de página si se llena
            if (y > pagina.Height - 100)
            {
                pagina = documento.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                y = 40;
                AñadirLogoEnCadaPagina(gfx, pagina);
                y += 100;
            }
        }

        y += 30;

        // Medicaciones
        gfx.DrawString("Medicaciones administradas", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 25;
        foreach (var m in medicaciones)
        {
            string linea = $"{m.fechaHora:dd/MM HH:mm} - {m.medicamento} ({m.dosis}), cada {m.frecuencia}. {m.observaciones}";
            gfx.DrawString(linea, fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 18;
        }

        y += 30;

        // Diuresis
        gfx.DrawString("Diuresis registrada", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 25;
        foreach (var d in diuresis)
        {
            string linea = $"{d.fechaHora:dd/MM HH:mm} - {d.cantidad} ml. {d.observaciones}";
            gfx.DrawString(linea, fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 18;
        }

        y += 30;

        // Cuidados de enfermería
        gfx.DrawString("Cuidados de enfermería", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 25;
        foreach (var c in cuidados)
        {
            string linea = $"{c.fechaHora:dd/MM HH:mm} - {c.descripcion}";
            gfx.DrawString(linea, fuenteTexto, XBrushes.Black, new XPoint(40, y)); y += 18;
        }

        y += 40;

        // Firma
        gfx.DrawString("Firmado por:", fuenteSeccion, XBrushes.Black, new XPoint(40, y)); y += 20;
        gfx.DrawLine(XPens.Black, 40, y, 250, y); y += 18;
        gfx.DrawString(nombreFirmante, fuenteTexto, XBrushes.Black, new XPoint(40, y));

        // Guarda el PDF en memoria y lo devuelve como array de bytes
        using var stream = new MemoryStream();
        documento.Save(stream, false);
        return stream.ToArray();
    }

    // Añade el logo institucional en la parte superior de cada página
    private void AñadirLogoEnCadaPagina(XGraphics gfx, PdfPage pagina)
    {
        var assembly = typeof(PdfGeneratorService).GetTypeInfo().Assembly;
        const string resourcePath = "FrontendMAUI.Resources.Images.umaer_logo_pdf.png";

        using var logoStream = assembly.GetManifestResourceStream(resourcePath);
        if (logoStream == null)
        {
            Debug.WriteLine($"No se encontró el recurso: {resourcePath}");
            return;
        }

        logoStream.Position = 0;

        using var logo = XImage.FromStream(() => logoStream);
        gfx.DrawImage(logo, 40, 30, 80, 80);
    }
}


