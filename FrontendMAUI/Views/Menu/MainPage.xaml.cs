using FrontendMAUI.Views.Auth;
using FrontendMAUI.Views.Configuracion;

namespace FrontendMAUI.Views.Menu;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        Console.WriteLine("App.EsAdmin: " + App.EsAdmin);

        BotonConfiguracion.IsVisible = App.EsAdmin;
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void OnConfigClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new Configuracion_Page());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de configuración.");
            Console.WriteLine($"[ERROR] Navegación a Configuración: {ex}");
        }
    }

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
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
            App.EsAdmin = false;
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cerrar la sesión correctamente.");
            Console.WriteLine($"[ERROR] Cerrar sesión: {ex}");
        }
    }

    public static implicit operator View(MainPage v)
    {
        throw new NotImplementedException();
    }
}
