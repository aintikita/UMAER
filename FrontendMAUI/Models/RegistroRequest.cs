namespace FrontendMAUI.Models
{
    public class RegistroRequest
    {
        public string usuario { get; set; } // Nombre de usuario
        public string contrasena { get; set; } // Contraseña del usuario
        public string adminPassword { get; set; } // Contraseña de administrador (si aplica)
    }
}

