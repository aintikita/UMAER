using System.Net.Http;
using System.Text.Json;
using FrontendMAUI.Models;
using FrontendMAUI.Configuración;

namespace FrontendMAUI.Services;

// Servicio para interactuar con el backend de "mochilas" y obtener archivos/carpetas
public static class MochilaService
{
    // Cliente HTTP para realizar las peticiones
    private static readonly HttpClient client = new HttpClient();

    // URL base del backend de mochilas
    private static readonly string baseUrl = $"{Config.BackendUrl}";

    // Obtiene el contenido (archivos y carpetas) de la ruta indicada en el backend de mochilas.
    // Si la ruta es nula, obtiene el contenido raíz.
    // Devuelve una lista de objetos ArchivoDocumento.

    public static async Task<List<ArchivoDocumento>> ObtenerContenidoAsync(string ruta)
    {
        var encodedRuta = Uri.EscapeDataString(ruta ?? "");
        var response = await client.GetAsync($"{baseUrl}/mochilas/contenido?ruta={encodedRuta}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var opciones = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<List<ArchivoDocumento>>(json, opciones);
    }
}

