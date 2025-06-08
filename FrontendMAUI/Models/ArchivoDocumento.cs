namespace FrontendMAUI.Models;

// Representa un archivo o carpeta en el sistema de la app
using System.Text.Json.Serialization;

public class ArchivoDocumento
{

    public string Nombre { get; set; }

    public bool EsCarpeta { get; set; }

    public string Ruta { get; set; }
}




