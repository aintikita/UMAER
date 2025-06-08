using System.Net.Http;
using System.Text;
using System.Text.Json;
using FrontendMAUI.Models;
using System.Collections.Generic;
using FrontendMAUI.Configuración;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace FrontendMAUI.Services
{
    // Servicio centralizado para interactuar con la API REST del backend
    public static class ApiService
    {
        // Cliente HTTP reutilizable
        private static readonly HttpClient client = new HttpClient();
        // URL base del backend
        private static readonly string baseUrl = $"{Config.BackendUrl}";

        // ------------------- AUTENTICACIÓN -------------------

        // Realiza login y devuelve el usuario autenticado
        public static async Task<User> LoginAsync(string usuario, string contrasena)
        {
            var body = new { usuario, contrasena };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{Config.BackendUrl}/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<User>(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Excepción al hacer login: {ex.Message}");
            }
            return null;
        }

        // ------------------- PACIENTES -------------------

        // Obtiene la lista de todos los pacientes
        public static async Task<List<Paciente>> GetPacientesAsync()
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/pacientes");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Paciente>>(json) ?? new List<Paciente>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al obtener pacientes: {ex.Message}");
                return new List<Paciente>();
            }
        }

        // Guarda un nuevo paciente
        public static async Task<Paciente> GuardarPacienteAsync(Paciente paciente)
        {
            var json = JsonSerializer.Serialize(paciente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/pacientes", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error guardando paciente: {response.StatusCode} - {errorMsg}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Paciente>(responseBody);
        }

        // Actualiza un paciente existente
        public static async Task<Paciente> ActualizarPacienteAsync(Paciente paciente)
        {
            if (paciente.Id == null)
                throw new ArgumentException("El Id del paciente no puede ser null");

            var json = JsonSerializer.Serialize(paciente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/pacientes/put/{paciente.Id}", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Paciente>(responseBody);
        }

        // Elimina un paciente por su Id
        public static async Task<bool> EliminarPacienteAsync(int pacienteId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/pacientes/{pacienteId}");
            return response.IsSuccessStatusCode;
        }

        // ------------------- CONSTANTES -------------------

        // Obtiene todas las constantes de un paciente
        public static async Task<List<Constante>> GetConstantesPorPacienteAsync(int pacienteId)
        {
            var response = await client.GetAsync($"{baseUrl}/constantes/{pacienteId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Constante>>(content) ?? new List<Constante>();
        }

        // Obtiene una constante específica por su Id
        public static async Task<Constante> GetConstantePorIdAsync(int id)
        {
            var response = await client.GetAsync($"{baseUrl}/constantes/constante/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Constante>(content);
        }

        // Guarda una nueva constante
        public static async Task<Constante> GuardarConstanteAsync(Constante constante)
        {
            var json = JsonSerializer.Serialize(constante);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/constantes", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error guardando constante: {response.StatusCode} - {errorMsg}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Constante>(responseBody)!;
        }

        // Actualiza una constante existente
        public static async Task<Constante> ActualizarConstanteAsync(Constante constante)
        {
            if (constante.id == null)
                throw new ArgumentException("El Id de la constante no puede ser null");

            var json = JsonSerializer.Serialize(constante);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/constantes/put/{constante.id}", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Constante>(responseBody);
        }

        // Elimina una constante por su Id
        public static async Task EliminarConstanteAsync(int constanteId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/constantes/{constanteId}");
            response.EnsureSuccessStatusCode();
        }

        // ------------------- MEDICACIÓN -------------------

        // Obtiene la medicación de un paciente
        public static async Task<List<Medicacion>> GetMedicacionPorPacienteAsync(int pacienteId)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/medicacion/{pacienteId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Medicacion>>(json) ?? new List<Medicacion>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al obtener medicación: {ex.Message}");
                return new List<Medicacion>();
            }
        }

        // Guarda una nueva medicación
        public static async Task<Medicacion> GuardarMedicacionAsync(Medicacion medicacion)
        {
            var json = JsonSerializer.Serialize(medicacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/medicacion", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Medicacion>(responseBody);
        }

        // Actualiza una medicación existente
        public static async Task ActualizarMedicacionAsync(Medicacion medicacion)
        {
            if (medicacion.id == null)
                throw new ArgumentException("El Id de la medicación no puede ser null");

            var json = JsonSerializer.Serialize(medicacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/medicacion/put/{medicacion.id}", content);
            response.EnsureSuccessStatusCode();
        }

        // Elimina una medicación por su Id
        public static async Task EliminarMedicacionAsync(int medicacionId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/medicacion/{medicacionId}");
            response.EnsureSuccessStatusCode();
        }

        // ------------------- DIURESIS -------------------

        // Obtiene la diuresis de un paciente
        public static async Task<List<Diuresis>> GetDiuresisPorPacienteAsync(int pacienteId)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/diuresis/{pacienteId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Diuresis>>(json) ?? new List<Diuresis>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al obtener diuresis: {ex.Message}");
                return new List<Diuresis>();
            }
        }

        // Guarda un nuevo registro de diuresis
        public static async Task<Diuresis> GuardarDiuresisAsync(Diuresis diuresis)
        {
            var json = JsonSerializer.Serialize(diuresis);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/diuresis", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Diuresis>(responseBody);
        }

        // Actualiza un registro de diuresis
        public static async Task ActualizarDiuresisAsync(Diuresis diuresis)
        {
            if (diuresis.id == null)
                throw new ArgumentException("El Id del registro no puede ser null");

            var json = JsonSerializer.Serialize(diuresis);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/diuresis/put/{diuresis.id}", content);
            response.EnsureSuccessStatusCode();
        }

        // Elimina un registro de diuresis
        public static async Task EliminarDiuresisAsync(int diuresisId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/diuresis/{diuresisId}");
            response.EnsureSuccessStatusCode();
        }

        // ------------------- BALANCE HÍDRICO -------------------

        // Obtiene el balance hídrico de un paciente
        public static async Task<List<BalanceHidrico>> GetBalancePorPacienteAsync(int pacienteId)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/balance/{pacienteId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BalanceHidrico>>(json) ?? new List<BalanceHidrico>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al obtener balance: {ex.Message}");
                return new List<BalanceHidrico>();
            }
        }

        // Guarda un nuevo balance hídrico
        public static async Task<BalanceHidrico> GuardarBalanceAsync(BalanceHidrico balance)
        {
            var json = JsonSerializer.Serialize(balance);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/balance", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BalanceHidrico>(responseBody);
        }

        // Actualiza un balance hídrico
        public static async Task ActualizarBalanceAsync(BalanceHidrico balance)
        {
            if (balance.id == null)
                throw new ArgumentException("El Id del balance no puede ser null");

            var json = JsonSerializer.Serialize(balance);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/balance/put/{balance.id}", content);
            response.EnsureSuccessStatusCode();
        }

        // Elimina un balance hídrico
        public static async Task EliminarBalanceAsync(int balanceId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/balance/{balanceId}");
            response.EnsureSuccessStatusCode();
        }

        // ------------------- ALERGIAS -------------------

        // Obtiene las alergias de un paciente
        public static async Task<List<Alergia>> GetAlergiasPorPacienteAsync(int pacienteId)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/alergias/{pacienteId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Alergia>>(json) ?? new List<Alergia>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al obtener alergias: {ex.Message}");
                return new List<Alergia>();
            }
        }

        // Guarda una nueva alergia
        public static async Task<Alergia> GuardarAlergiaAsync(Alergia alergia)
        {
            var json = JsonSerializer.Serialize(alergia);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/alergias", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error guardando alergia: {response.StatusCode} - {errorMsg}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Alergia>(responseBody);
        }

        // Actualiza una alergia
        public static async Task ActualizarAlergiaAsync(Alergia alergia)
        {
            if (alergia.id == null)
                throw new ArgumentException("El Id de la alergia no puede ser null");

            var json = JsonSerializer.Serialize(alergia);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/alergias/put/{alergia.id}", content);
            response.EnsureSuccessStatusCode();
        }

        // Elimina una alergia
        public static async Task EliminarAlergiaAsync(int alergiaId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/alergias/{alergiaId}");
            response.EnsureSuccessStatusCode();
        }

        // ------------------- CUIDADOS DE ENFERMERÍA -------------------

        // Obtiene los cuidados de enfermería de un paciente
        public static async Task<List<CuidadoEnfermeria>> GetCuidadosPorPacienteAsync(int pacienteId)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/cuidados/{pacienteId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CuidadoEnfermeria>>(json) ?? new List<CuidadoEnfermeria>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error al obtener cuidados: {ex.Message}");
                return new List<CuidadoEnfermeria>();
            }
        }

        // Guarda un nuevo cuidado de enfermería
        public static async Task<CuidadoEnfermeria> GuardarCuidadoAsync(CuidadoEnfermeria cuidado)
        {
            var json = JsonSerializer.Serialize(cuidado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/cuidados", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CuidadoEnfermeria>(responseBody);
        }

        // Actualiza un cuidado de enfermería
        public static async Task ActualizarCuidadoAsync(CuidadoEnfermeria cuidado)
        {
            if (cuidado.id == null)
                throw new ArgumentException("El Id del cuidado no puede ser null");

            var json = JsonSerializer.Serialize(cuidado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{baseUrl}/cuidados/put/{cuidado.id}", content);
            response.EnsureSuccessStatusCode();
        }

        // Elimina un cuidado de enfermería
        public static async Task EliminarCuidadoAsync(int cuidadoId)
        {
            var response = await client.DeleteAsync($"{baseUrl}/cuidados/{cuidadoId}");
            response.EnsureSuccessStatusCode();
        }

        // ------------------- REPORTES PDF -------------------

        // Descarga un PDF generado por el backend
        public static async Task<byte[]> DescargarPdfAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/reportes/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        // Genera un reporte PDF en el backend
        public static async Task<ReportePdf> GenerarReporteAsync(int pacienteId, List<string> fechas, List<double> valores, string descripcion)
        {
            var body = new
            {
                pacienteId = pacienteId,
                fechas = fechas,
                valores = valores,
                descripcion = descripcion
            };

            var response = await client.PostAsJsonAsync($"{baseUrl}/pdf/generar", body);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error generando reporte: {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<ReportePdf>();
        }

        // Obtiene la lista de reportes PDF guardados
        public static async Task<List<string>> ObtenerReportesAsync()
        {
            var response = await client.GetAsync($"{baseUrl}/pdf/lista");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
        }

        // Devuelve la URL para descargar un reporte PDF específico
        public static string ObtenerUrlDescarga(string nombreArchivo)
        {
            return $"{baseUrl}/pdf/descargar/{nombreArchivo}";
        }
    }
}