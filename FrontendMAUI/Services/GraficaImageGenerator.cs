using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using FrontendMAUI.Models;
using FrontendMAUI;

// Genera una imagen PNG de una gráfica a partir de una lista de constantes y tipos seleccionados
public static class GraficaImageGenerator
{
    // Genera la gráfica y la devuelve como un Stream PNG
    public static Stream GenerarGrafica(List<Constante> registros, string[] tipos)
    {
        const int width = 800;
        const int height = 400;

        // Márgenes para dejar espacio a los ejes y leyendas
        int marginLeft = 80;   // Espacio para los números del eje Y
        int marginTop = 60;
        int marginBottom = 50;
        int marginRight = 20;

        var bitmap = new SKBitmap(width, height);
        var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        // Pincel para ejes y marcas
        var paintEjes = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 2,
            IsAntialias = true
        };

        // Pincel para los textos (números y leyendas)
        var paintTexto = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 14,
            IsAntialias = true
        };

        // Dibujar eje Y con marcas y valores numéricos
        int numDivisiones = 5;
        for (int i = 0; i <= numDivisiones; i++)
        {
            float y = marginTop + i * (float)(height - marginTop - marginBottom) / numDivisiones;
            canvas.DrawLine(marginLeft - 5, y, marginLeft, y, paintEjes);

            // Calcula el valor correspondiente a la marca (de mayor a menor)
            double valor = ((numDivisiones - i) * 1.0) / numDivisiones * ObtenerMaxValor(registros);
            string texto = valor.ToString("0");
            canvas.DrawText(texto, marginLeft - 10 - paintTexto.MeasureText(texto), y + paintTexto.TextSize / 2, paintTexto);
        }

        // Dibuja los ejes principales
        canvas.DrawLine(marginLeft, marginTop, marginLeft, height - marginBottom, paintEjes); // Eje Y
        canvas.DrawLine(marginLeft, height - marginBottom, width - marginRight, height - marginBottom, paintEjes); // Eje X

        // Colores para cada tipo de constante
        var colores = new Dictionary<string, SKColor>
        {
            { "Temperatura", SKColors.Orange },
            { "Frecuencia Cardíaca", SKColors.Red },
            { "Frecuencia Respiratoria", SKColors.Green },
            { "Tensión Sistólica", SKColors.Blue },
            { "Tensión Diastólica", SKColors.Purple },
            { "Saturación", SKColors.Teal },
            { "Glucemia", SKColors.Brown }
        };

        int total = registros.Count;
        if (total < 2) return new MemoryStream(); // No se puede graficar con menos de 2 puntos

        double maxValor = ObtenerMaxValor(registros);

        // Dibuja cada tipo de constante seleccionado
        foreach (var tipo in tipos)
        {
            var puntos = new List<SKPoint>();
            for (int j = 0; j < total; j++)
            {
                var r = registros[j];
                // Selecciona el valor según el tipo
                double valor = tipo switch
                {
                    "Temperatura" => r.temperatura ?? 0,
                    "Frecuencia Cardíaca" => r.frecuenciaCardiaca ?? 0,
                    "Frecuencia Respiratoria" => r.frecuenciaRespiratoria ?? 0,
                    "Tensión Sistólica" => ExtraerPresion(r.presionArterial, true),
                    "Tensión Diastólica" => ExtraerPresion(r.presionArterial, false),
                    "Saturación" => r.saturacionOxigeno ?? 0,
                    _ => 0
                };

                // Calcula la posición X e Y del punto en la gráfica
                float x = marginLeft + j * ((width - marginLeft - marginRight) / (float)(total - 1));
                float y = (float)(height - marginBottom - (valor / maxValor) * (height - marginBottom - marginTop));
                puntos.Add(new SKPoint(x, y));
            }

            // Dibuja la línea de la serie
            var paintLinea = new SKPaint
            {
                Color = colores[tipo],
                StrokeWidth = 2,
                IsAntialias = true
            };

            for (int k = 1; k < puntos.Count; k++)
                canvas.DrawLine(puntos[k - 1], puntos[k], paintLinea);
        }

        // Dibuja la leyenda de colores y tipos en la parte superior derecha
        int leyendaY = marginTop - 30;
        foreach (var tipo in tipos)
        {
            var leyendaPaint = new SKPaint
            {
                Color = colores[tipo],
                TextSize = 16,
                IsAntialias = true
            };
            canvas.DrawText(tipo, width - 180, leyendaY, leyendaPaint);
            leyendaY += 20;
        }

        // Convierte el bitmap a PNG y lo devuelve como Stream
        var img = SKImage.FromBitmap(bitmap);
        var data = img.Encode(SKEncodedImageFormat.Png, 100);
        var stream = new MemoryStream();
        data.SaveTo(stream);
        stream.Position = 0;
        return stream;
    }

    // Obtiene el valor máximo entre todas las constantes para escalar la gráfica
    private static double ObtenerMaxValor(List<Constante> registros)
    {
        return registros.Max(r => new double?[]
        {
            r.temperatura ?? 0,
            r.frecuenciaCardiaca ?? 0,
            r.frecuenciaRespiratoria ?? 0,
            ExtraerPresion(r.presionArterial, true),
            ExtraerPresion(r.presionArterial, false),
            r.saturacionOxigeno ?? 0
        }.Max() ?? 1);
    }

    // Extrae la presión sistólica o diastólica de un string tipo "120/80"
    private static int ExtraerPresion(string presion, bool sistolica)
    {
        if (string.IsNullOrWhiteSpace(presion)) return 0;
        var partes = presion.Split('/');
        if (partes.Length == 2)
        {
            return int.TryParse(sistolica ? partes[0] : partes[1], out var val) ? val : 0;
        }
        return 0;
    }
}