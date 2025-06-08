using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Calculadoras;

// P�gina para calcular la duraci�n de ox�geno disponible seg�n los par�metros ingresados
public partial class CalculoHorasPage : ContentPage
{
    // Constructor: inicializa la vista
    public CalculoHorasPage()
    {
        InitializeComponent();
    }

    // Evento que se ejecuta al pulsar el bot�n de calcular
    // Lee los valores de los campos, llama al servicio y muestra el resultado
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
        try
        {
            // Obtiene y convierte los valores de los campos de entrada
            double litros = double.Parse(LitrosBotellaEntry.Text);
            double numBotellas = double.Parse(NumeroBotellasEntry.Text);
            double presion = double.Parse(PresionBotellaEntry.Text);
            double consumo = double.Parse(ConsumoOxigenoEntry.Text);

            // Llama al servicio de c�lculo y espera el resultado
            var resultado = await CalculadoraService.CalcularHorasAsync(litros, numBotellas, presion, consumo);

            // Muestra el resultado en la etiqueta
            ResultadoLabel.Text = $"Duraci�n estimada: {resultado} horas";
        }
        catch
        {
            // Si hay error en la conversi�n o c�lculo, muestra mensaje de error
            ResultadoLabel.Text = "Error: valores inv�lidos.";
        }
    }
}


