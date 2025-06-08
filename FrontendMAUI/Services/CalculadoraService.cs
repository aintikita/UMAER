using System.Net.Http;
using System.Text;
using System.Text.Json;
using FrontendMAUI.Configuración;
using System.Threading.Tasks;

namespace FrontendMAUI.Services
{
    // Servicio para realizar cálculos médicos a través del backend
    public static class CalculadoraService
    {
        // Cliente HTTP para las peticiones
        private static readonly HttpClient client = new HttpClient();

        // URL base del backend (ajusta según tu entorno)
        private static readonly string baseUrl = $"{Config.BackendUrl}"; // Cámbialo por tu IP real

        // Calcula cuántas botellas de oxígeno se necesitan para un vuelo
        // horasVuelo: duración del vuelo en horas
        // consumoOxigeno: consumo de oxígeno por hora
        // presionBotella: presión de cada botella
        // litrosBotella: capacidad de cada botella en litros
        public static async Task<string> CalcularBotellasAsync(double horasVuelo, double consumoOxigeno, double presionBotella, double litrosBotella)
        {
            var body = new
            {
                horasVuelo,
                consumoOxigeno,
                presionBotella,
                litrosBotella
            };

            // Llama al backend y devuelve el resultado
            return await PostResultado("/calculadora/botellas", body);
        }

        // Calcula cuántas horas de oxígeno se tienen disponibles
        // litrosBotella: capacidad de cada botella
        // numeroBotellas: cantidad de botellas
        // presionBotella: presión de cada botella
        // consumoOxigeno: consumo de oxígeno por hora
        public static async Task<string> CalcularHorasAsync(double litrosBotella, double numeroBotellas, double presionBotella, double consumoOxigeno)
        {
            var body = new
            {
                litrosBotella,
                numeroBotellas,
                presionBotella,
                consumoOxigeno
            };

            // Llama al backend y devuelve el resultado
            return await PostResultado("/calculadora/horas", body);
        }

        // Calcula la dosis total de medicamento según el peso
        // dosisPorKg: dosis por kilogramo
        // peso: peso del paciente
        public static async Task<string> CalcularDosisAsync(double dosisPorKg, double peso)
        {
            var body = new
            {
                dosisPorKg,
                peso
            };

            // Llama al backend y devuelve el resultado
            return await PostResultado("/calculadora/dosis", body);
        }

        // Realiza una petición POST al backend y devuelve el resultado del cálculo
        // endpoint: ruta del cálculo en el backend
        // body: objeto con los datos necesarios para el cálculo
        private static async Task<string> PostResultado(string endpoint, object body)
        {
            try
            {
                // Serializa el cuerpo a JSON
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Envía la petición POST
                var response = await client.PostAsync($"{baseUrl}{endpoint}", content);

                // Si la respuesta es exitosa, extrae el resultado
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(result);
                    return doc.RootElement.GetProperty("resultado").GetString();
                }
                else
                {
                    return "Error de cálculo";
                }
            }
            catch (Exception ex)
            {
                // Devuelve el mensaje de error si ocurre una excepción
                return $"Error: {ex.Message}";
            }
        }
    }
}