namespace FrontendMAUI.Views.Calculadoras;

// Página principal para seleccionar el tipo de cálculo de oxígeno
public partial class CalculadoraO2Page : ContentPage
{
    public CalculadoraO2Page()
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, true); // Asegura que el botón atrás se muestre
        NavigationPage.SetHasNavigationBar(this, true); // Asegura que la barra se muestre
    }

    // Método auxiliar para mostrar mensajes de error
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    // Navega a la página para calcular cuántas botellas de oxígeno se necesitan
    private async void OnCalculoBotellasClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new CalculoBotellasPage());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la calculadora de botellas de oxígeno.");
            Console.WriteLine($"[ERROR] {ex}");
        }
    }

    // Navega a la página para calcular la duración de oxígeno disponible
    private async void OnCalculoHorasClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new CalculoHorasPage());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la calculadora de horas de oxígeno.");
            Console.WriteLine($"[ERROR] {ex}");
        }
    }
}

