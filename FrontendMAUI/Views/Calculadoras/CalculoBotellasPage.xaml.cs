using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Calculadoras;

// P�gina para calcular cu�ntas botellas de ox�geno se necesitan para un vuelo
public partial class CalculoBotellasPage : ContentPage
{
    public CalculoBotellasPage()
    {
        InitializeComponent();
    }

    // M�todo auxiliar para mostrar mensajes de error al usuario
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    // Evento que se ejecuta al pulsar el bot�n de calcular
    private async void OnCalculateButtonClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        ResultadoLabel.Text = string.Empty;

        // Validaci�n de campos vac�os
        if (string.IsNullOrWhiteSpace(HorasDeVueloEntry.Text) ||
            string.IsNullOrWhiteSpace(ConsumoOxigenoEntry.Text) ||
            string.IsNullOrWhiteSpace(PresionBotellaEntry.Text) ||
            string.IsNullOrWhiteSpace(LitrosBotellaEntry.Text))
        {
            await MostrarErrorAsync("Por favor, rellena todos los campos.");
            return;
        }

        // Validaci�n de valores num�ricos y positivos
        if (!double.TryParse(HorasDeVueloEntry.Text, out double horas) || horas <= 0)
        {
            await MostrarErrorAsync("Las horas de vuelo deben ser un n�mero positivo.");
            return;
        }
        if (!double.TryParse(ConsumoOxigenoEntry.Text, out double consumo) || consumo <= 0)
        {
            await MostrarErrorAsync("El consumo de ox�geno debe ser un n�mero positivo.");
            return;
        }
        if (!double.TryParse(PresionBotellaEntry.Text, out double presion) || presion <= 0)
        {
            await MostrarErrorAsync("La presi�n de la botella debe ser un n�mero positivo.");
            return;
        }
        if (!double.TryParse(LitrosBotellaEntry.Text, out double litros) || litros <= 0)
        {
            await MostrarErrorAsync("Los litros de la botella deben ser un n�mero positivo.");
            return;
        }

        try
        {
            // Llama al servicio de c�lculo y espera el resultado
            var resultado = await CalculadoraService.CalcularBotellasAsync(horas, consumo, presion, litros);

            // Muestra el resultado en la etiqueta
            ResultadoLabel.Text = $"Resultado: {resultado}";
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurri� un error al calcular las botellas. Int�ntalo de nuevo.");
            Console.WriteLine($"[ERROR] {ex}");
        }
    }

    // No implementado, no se utiliza en la l�gica de la p�gina
    public static implicit operator View(CalculoBotellasPage v)
    {
        throw new NotImplementedException();
    }
}

