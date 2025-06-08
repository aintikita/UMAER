using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FrontendMAUI.Configuración;
using System.Threading.Tasks;
using FrontendMAUI.Configuración;

namespace FrontendMAUI.Services
{
    // Servicio para interactuar con la API de PDFs del backend
    public class PdfApiService
    {
        private readonly HttpClient _http = new(); // Cliente HTTP para las peticiones

        // Devuelve una lista de nombres de archivos PDF en la carpeta indicada
        public async Task<List<string>> ListarPdfs(string carpeta)
        {
            var url = $"{Config.BackendUrl}/pdf/listar?carpeta={carpeta}";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        // Devuelve el contenido de texto de un PDF específico
        public async Task<string> ObtenerTexto(string carpeta, string nombre)
        {
            var url = $"{Config.BackendUrl}/pdf/leer?carpeta={carpeta}&nombre={nombre}";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return $"Error al obtener PDF: {response.StatusCode}";

            return await response.Content.ReadAsStringAsync();
        }
    }

}

