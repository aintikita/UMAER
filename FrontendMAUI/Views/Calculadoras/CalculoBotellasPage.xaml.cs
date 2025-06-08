using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Calculadoras;

// Página para calcular cuántas botellas de oxígeno se necesitan para un vuelo
public partial class CalculoBotellasPage : ContentPage
{
    public CalculoBotellasPage()
    {
        InitializeComponent();
    }

    // Método auxiliar para mostrar mensajes de error al usuario
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    // Evento que se ejecuta al pulsar el botón de calcular
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

        // Validación de campos vacíos
        if (string.IsNullOrWhiteSpace(HorasDeVueloEntry.Text) ||
            string.IsNullOrWhiteSpace(ConsumoOxigenoEntry.Text) ||
            string.IsNullOrWhiteSpace(PresionBotellaEntry.Text) ||
            string.IsNullOrWhiteSpace(LitrosBotellaEntry.Text))
        {
            await MostrarErrorAsync("Por favor, rellena todos los campos.");
            return;
        }

        // Validación de valores numéricos y positivos
        if (!double.TryParse(HorasDeVueloEntry.Text, out double horas) || horas <= 0)
        {
            await MostrarErrorAsync("Las horas de vuelo deben ser un número positivo.");
            return;
        }
        if (!double.TryParse(ConsumoOxigenoEntry.Text, out double consumo) || consumo <= 0)
        {
            await MostrarErrorAsync("El consumo de oxígeno debe ser un número positivo.");
            return;
        }
        if (!double.TryParse(PresionBotellaEntry.Text, out double presion) || presion <= 0)
        {
            await MostrarErrorAsync("La presión de la botella debe ser un número positivo.");
            return;
        }
        if (!double.TryParse(LitrosBotellaEntry.Text, out double litros) || litros <= 0)
        {
            await MostrarErrorAsync("Los litros de la botella deben ser un número positivo.");
            return;
        }

        try
        {
            // Llama al servicio de cálculo y espera el resultado
            var resultado = await CalculadoraService.CalcularBotellasAsync(horas, consumo, presion, litros);

            // Muestra el resultado en la etiqueta
            ResultadoLabel.Text = $"Resultado: {resultado}";
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error al calcular las botellas. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] {ex}");
        }
    }

    // No implementado, no se utiliza en la lógica de la página
    public static implicit operator View(CalculoBotellasPage v)
    {
        throw new NotImplementedException();
    }
}

