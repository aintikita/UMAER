using FrontendMAUI.Views.Auth;

namespace FrontendMAUI.Views.PaginaPrincipal;

public partial class PaginaPrincipalPage : ContentPage
{
    public PaginaPrincipalPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void OnGetStartedClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        try
        {
            await Navigation.PushAsync(new LoginPage());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de inicio de sesión.");
            Console.WriteLine($"[ERROR] Navegación a LoginPage: {ex}");
        }
    }

    private async void OnSignInTapped(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de registro.");
            Console.WriteLine($"[ERROR] Navegación a RegisterPage: {ex}");
        }
    }
}
