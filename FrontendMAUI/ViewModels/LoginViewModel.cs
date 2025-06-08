using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using FrontendMAUI.Services;
using Microsoft.Maui.Controls;

namespace FrontendMAUI.ViewModels
{
    // ViewModel para la pantalla de Login, implementa INotifyPropertyChanged para binding
    public class LoginViewModel : INotifyPropertyChanged
    {
        // Evento para notificar cambios en las propiedades al UI
        public event PropertyChangedEventHandler PropertyChanged;

        private string usuario;
        private string contrasena;
        private string mensaje;
        private bool isBusy;

        // Propiedad Usuario con notificación de cambios
        public string Usuario
        {
            get => usuario;
            set
            {
                if (usuario != value)
                {
                    usuario = value;
                    OnPropertyChanged();
                }
            }
        }

        // Propiedad Contraseña con notificación de cambios
        public string Contrasena
        {
            get => contrasena;
            set
            {
                if (contrasena != value)
                {
                    contrasena = value;
                    OnPropertyChanged();
                }
            }
        }

        // Mensaje de feedback para el usuario (errores o información)
        public string Mensaje
        {
            get => mensaje;
            set
            {
                if (mensaje != value)
                {
                    mensaje = value;
                    OnPropertyChanged();
                }
            }
        }

        // Indica si el login está en proceso, para deshabilitar el botón y evitar clics múltiples
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                    ((Command)LoginCommand).ChangeCanExecute();
                }
            }
        }

        // Comando vinculado al botón Login
        public ICommand LoginCommand { get; }

        // Constructor: inicializa el comando con función async y control de disponibilidad
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
        }

        // Método principal para realizar el login
        private async Task LoginAsync()
        {
            if (IsBusy) return; // Evita ejecución si ya está en proceso

            try
            {
                IsBusy = true;
                Mensaje = string.Empty;

                // Validaciones básicas de formulario
                if (string.IsNullOrWhiteSpace(Usuario))
                {
                    Mensaje = "El usuario es obligatorio.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Contrasena))
                {
                    Mensaje = "La contraseña es obligatoria.";
                    return;
                }

                // Llama al servicio para validar credenciales
                var resultado = await ApiService.LoginAsync(Usuario, Contrasena);

                if (resultado != null)
                {
                    // Aquí puedes usar la propiedad Admin del resultado si la tienes del backend
                    App.EsAdmin = resultado.Admin;

                    // O, si haces login hardcoded (temporal), por ejemplo:
                    if (Usuario == "admin" && Contrasena == "admin123")
                    {
                        App.EsAdmin = true;
                    }
                    else
                    {
                        App.EsAdmin = false;
                    }

                    // Navegar a la página principal
                    Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//MainPage");
                }

                else
                {
                    // Credenciales inválidas
                    Mensaje = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                // Captura cualquier error inesperado y lo muestra
                Mensaje = $"Error al iniciar sesión: {ex.Message}";
            }
            finally
            {
                // Siempre libera la bandera IsBusy
                IsBusy = false;
            }
        }

        // Método helper para notificar cambios de propiedad automáticamente
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}




