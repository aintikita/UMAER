using System.Text;
using System.Text.Json.Serialization;

namespace FrontendMAUI.Models
{
    public class Usuario
    {
        public int id { get; set; } // Id del usuario
        public string nombreUsuario { get; set; } // Nombre completo o usuario
        public bool esAdmin { get; set; } // Es administrador

        public string NombreIniciales // Iniciales del nombre
        {
            get
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                    return "";

                var palabras = nombreUsuario.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var iniciales = new System.Text.StringBuilder();

                foreach (var palabra in palabras)
                    iniciales.Append(char.ToUpper(palabra[0]));

                return iniciales.ToString();
            }
        }

        public bool IsSeleccionado { get; set; } // Solo visual, no viene del backend
    }
}
