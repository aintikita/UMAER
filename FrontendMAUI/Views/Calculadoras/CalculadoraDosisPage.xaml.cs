using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Calculadoras;

// P�gina para calcular la dosis total de un medicamento seg�n el peso del paciente
public partial class CalculadoraDosisPage : ContentPage
{
    public CalculadoraDosisPage()
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
        if (string.IsNullOrWhiteSpace(DosisPorKgEntry.Text) || string.IsNullOrWhiteSpace(PesoEntry.Text))
        {
            await MostrarErrorAsync("Por favor, rellena todos los campos.");
            return;
        }

        // Validaci�n de valores num�ricos y positivos
        if (!double.TryParse(DosisPorKgEntry.Text, out double dosisPorKg) || dosisPorKg <= 0)
        {
            await MostrarErrorAsync("La dosis por kg debe ser un n�mero positivo.");
            return;
        }

        if (!double.TryParse(PesoEntry.Text, out double peso) || peso <= 0)
        {
            await MostrarErrorAsync("El peso debe ser un n�mero positivo.");
            return;
        }

        try
        {
            // Llama al servicio de c�lculo y espera el resultado
            var resultado = await CalculadoraService.CalcularDosisAsync(dosisPorKg, peso);

            // Muestra el resultado en la etiqueta
            ResultadoLabel.Text = $"Dosis calculada: {resultado} mg";
        }
        catch (Exception ex)
        {
            // Si hay error en el c�lculo, muestra mensaje de error gen�rico
            await MostrarErrorAsync("Ocurri� un error al calcular la dosis. Int�ntalo de nuevo.");
            // Opcional: log interno para depuraci�n
            Console.WriteLine($"[ERROR] {ex}");
        }
    }
}



