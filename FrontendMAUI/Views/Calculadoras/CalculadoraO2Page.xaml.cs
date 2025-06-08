namespace FrontendMAUI.Views.Calculadoras;

// P�gina principal para seleccionar el tipo de c�lculo de ox�geno
public partial class CalculadoraO2Page : ContentPage
{
    public CalculadoraO2Page()
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, true); // Asegura que el bot�n atr�s se muestre
        NavigationPage.SetHasNavigationBar(this, true); // Asegura que la barra se muestre
    }

    // M�todo auxiliar para mostrar mensajes de error
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    // Navega a la p�gina para calcular cu�ntas botellas de ox�geno se necesitan
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
            await MostrarErrorAsync("No se pudo abrir la calculadora de botellas de ox�geno.");
            Console.WriteLine($"[ERROR] {ex}");
        }
    }

    // Navega a la p�gina para calcular la duraci�n de ox�geno disponible
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
            await MostrarErrorAsync("No se pudo abrir la calculadora de horas de ox�geno.");
            Console.WriteLine($"[ERROR] {ex}");
        }
    }
}

