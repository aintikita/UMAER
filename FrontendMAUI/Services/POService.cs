using System.Net.Http;
using System.Text.Json;
using FrontendMAUI.Models;
using FrontendMAUI.Configuración;

namespace FrontendMAUI.Services;

// Servicio para obtener el contenido de archivos y carpetas desde el backend PO
public static class POService
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string baseUrl = $"{Config.BackendUrl}";

    public static async Task<List<ArchivoDocumento>> ObtenerContenidoAsync(string ruta)
    {
        var encodedRuta = Uri.EscapeDataString(ruta ?? "");
        var response = await client.GetAsync($"{baseUrl}/po/contenido?ruta={encodedRuta}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var opciones = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<List<ArchivoDocumento>>(json, opciones);
    }
}




