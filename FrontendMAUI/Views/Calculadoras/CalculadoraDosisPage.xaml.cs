using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Calculadoras;

// Página para calcular la dosis total de un medicamento según el peso del paciente
public partial class CalculadoraDosisPage : ContentPage
{
    public CalculadoraDosisPage()
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
        if (string.IsNullOrWhiteSpace(DosisPorKgEntry.Text) || string.IsNullOrWhiteSpace(PesoEntry.Text))
        {
            await MostrarErrorAsync("Por favor, rellena todos los campos.");
            return;
        }

        // Validación de valores numéricos y positivos
        if (!double.TryParse(DosisPorKgEntry.Text, out double dosisPorKg) || dosisPorKg <= 0)
        {
            await MostrarErrorAsync("La dosis por kg debe ser un número positivo.");
            return;
        }

        if (!double.TryParse(PesoEntry.Text, out double peso) || peso <= 0)
        {
            await MostrarErrorAsync("El peso debe ser un número positivo.");
            return;
        }

        try
        {
            // Llama al servicio de cálculo y espera el resultado
            var resultado = await CalculadoraService.CalcularDosisAsync(dosisPorKg, peso);

            // Muestra el resultado en la etiqueta
            ResultadoLabel.Text = $"Dosis calculada: {resultado} mg";
        }
        catch (Exception ex)
        {
            // Si hay error en el cálculo, muestra mensaje de error genérico
            await MostrarErrorAsync("Ocurrió un error al calcular la dosis. Inténtalo de nuevo.");
            // Opcional: log interno para depuración
            Console.WriteLine($"[ERROR] {ex}");
        }
    }
}



