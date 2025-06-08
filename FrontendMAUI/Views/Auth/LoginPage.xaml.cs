using Microsoft.Maui.Controls;

namespace FrontendMAUI.Views.Auth
{
    // Página de inicio de sesión (login) para el usuario
    public partial class LoginPage : ContentPage
    {
        // Constructor: inicializa la vista y oculta la barra de navegación
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // Evento que se ejecuta al pulsar el botón "Registrarse"
        // Navega a la página de registro de usuario
        private async void OnRegistrarseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void OnContrasenaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OlvidarContrasenaPage());
        }
    }
}



