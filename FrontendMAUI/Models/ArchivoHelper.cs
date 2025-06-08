using System.Net.Http;

namespace FrontendMAUI.Models
{
    // Métodos auxiliares para trabajar con archivos PDF en la app
    public static class ArchivoHelper
    {
        // Cliente HTTP reutilizable para descargar archivos
        private static readonly HttpClient client = new();

        // Descarga un PDF desde una URL y lo guarda localmente si no existe.
        // Si ya existe, devuelve la ruta local directamente.
        // Lanza excepción si la descarga falla o el archivo está vacío.
        public static async Task<string> DescargarPdfLocalAsync(string nombreArchivo, string url)
        {
            // Obtiene la carpeta de almacenamiento de la app
            var documents = FileSystem.AppDataDirectory;
            // Ruta completa donde se guardará el archivo
            var filePath = Path.Combine(documents, nombreArchivo);

            try
            {
                // Si el archivo no existe, lo descarga
                if (!File.Exists(filePath))
                {
                    // Descarga el archivo como array de bytes
                    var bytes = await client.GetByteArrayAsync(url);

                    // Si la descarga está vacía, lanza excepción
                    if (bytes == null || bytes.Length == 0)
                        throw new Exception("El archivo descargado está vacío.");

                    // Guarda el archivo en la ruta especificada
                    File.WriteAllBytes(filePath, bytes);
                }

                // Verifica que el archivo exista después de guardar
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("No se pudo guardar el PDF en el dispositivo.");

                // Devuelve la ruta local del archivo
                return filePath;
            }
            catch (Exception ex)
            {
                // Cualquier error se propaga con un mensaje personalizado
                throw new Exception($"Error al descargar PDF: {ex.Message}");
            }
        }
    }
}




