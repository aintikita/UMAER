// Services/ManualMedicinaService.cs
using System.Net.Http;
using System.Text.Json;
using FrontendMAUI.Configuración;
using FrontendMAUI.Models;

namespace FrontendMAUI.Services;

// Servicio para obtener archivos y carpetas de manuales de medicinas desde el backend
public static class ManualMedicinaService
{
    // Cliente HTTP para las peticiones al backend
    private static readonly HttpClient client = new HttpClient();

    // URL base del backend de manuales de medicinas
    private static readonly string baseUrl = $"{Config.BackendUrl}";

    // Obtiene el contenido (archivos y carpetas) de la ruta indicada.
    // Si la ruta es nula, obtiene el contenido raíz.
    // Devuelve una lista de objetos ArchivoDocumento.

    public static async Task<List<ArchivoDocumento>> ObtenerContenidoAsync(string ruta)
    {
        var encodedRuta = Uri.EscapeDataString(ruta ?? "");
        var response = await client.GetAsync($"{baseUrl}/manuales_medicinas/contenido?ruta={encodedRuta}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var opciones = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<List<ArchivoDocumento>>(json, opciones);
    }
}



