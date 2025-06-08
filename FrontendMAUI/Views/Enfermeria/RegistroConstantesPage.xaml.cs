using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroConstantesPage : ContentPage
{
    private readonly int _pacienteId;
    private Constante _constanteEditar;

    public RegistroConstantesPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        TituloLabel.Text = "Añadir Constante";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    // Constructor para editar existente
    public RegistroConstantesPage(Constante constante) : this(constante.pacienteId)
    {
        _constanteEditar = constante;
        TituloLabel.Text = "Editar Constante";

        // Cargar datos en el formulario
        TemperaturaEntry.Text = constante.temperatura?.ToString() ?? "";
        FrecuenciaCardiacaEntry.Text = constante.frecuenciaCardiaca?.ToString() ?? "";
        FrecuenciaRespiratoriaEntry.Text = constante.frecuenciaRespiratoria?.ToString() ?? "";
        SaturacionOxigenoEntry.Text = constante.saturacionOxigeno?.ToString() ?? "";
        PresionArterialEntry.Text = constante.presionArterial ?? "";
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        // Validación de campos numéricos y obligatorios
        float? temperatura = null;
        int? frecuenciaCardiaca = null;
        int? frecuenciaRespiratoria = null;
        int? saturacionOxigeno = null;
        string presionArterial = PresionArterialEntry.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(TemperaturaEntry.Text))
        {
            if (!float.TryParse(TemperaturaEntry.Text, out var temp) || temp <= 0)
            {
                await MostrarErrorAsync("La temperatura debe ser un número positivo.");
                return;
            }
            temperatura = temp;
        }

        if (!string.IsNullOrWhiteSpace(FrecuenciaCardiacaEntry.Text))
        {
            if (!int.TryParse(FrecuenciaCardiacaEntry.Text, out var fc) || fc <= 0)
            {
                await MostrarErrorAsync("La frecuencia cardiaca debe ser un número entero positivo.");
                return;
            }
            frecuenciaCardiaca = fc;
        }

        if (!string.IsNullOrWhiteSpace(FrecuenciaRespiratoriaEntry.Text))
        {
            if (!int.TryParse(FrecuenciaRespiratoriaEntry.Text, out var fr) || fr <= 0)
            {
                await MostrarErrorAsync("La frecuencia respiratoria debe ser un número entero positivo.");
                return;
            }
            frecuenciaRespiratoria = fr;
        }

        if (!string.IsNullOrWhiteSpace(SaturacionOxigenoEntry.Text))
        {
            if (!int.TryParse(SaturacionOxigenoEntry.Text, out var so) || so <= 0 || so > 100)
            {
                await MostrarErrorAsync("La saturación de oxígeno debe ser un número entre 1 y 100.");
                return;
            }
            saturacionOxigeno = so;
        }

        // Al menos un campo debe estar informado
        if (temperatura == null && frecuenciaCardiaca == null && frecuenciaRespiratoria == null && saturacionOxigeno == null && string.IsNullOrWhiteSpace(presionArterial))
        {
            await MostrarErrorAsync("Debes introducir al menos un valor de constante.");
            return;
        }

        var constante = new Constante
        {
            pacienteId = _pacienteId,
            fechaHora = DateTime.Now,
            temperatura = temperatura,
            frecuenciaCardiaca = frecuenciaCardiaca,
            frecuenciaRespiratoria = frecuenciaRespiratoria,
            saturacionOxigeno = saturacionOxigeno,
            presionArterial = presionArterial
        };

        try
        {
            if (_constanteEditar != null)
            {
                constante.id = _constanteEditar.id;
                await ApiService.ActualizarConstanteAsync(constante);
            }
            else
            {
                await ApiService.GuardarConstanteAsync(constante);
            }

            await DisplayAlert("Éxito", "Constante guardada", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar la constante. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar constante: {ex}");
        }
    }
}


