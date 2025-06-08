using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroMedicacionPage : ContentPage
{
    private readonly int _pacienteId;
    private Medicacion _medicacionEditar;

    public RegistroMedicacionPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        TituloLabel.Text = "Añadir Medicacion";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    public RegistroMedicacionPage(Medicacion medicacion) : this(medicacion.pacienteId)
    {
        _medicacionEditar = medicacion;
        TituloLabel.Text = "Editar Medicacion";
        MedicamentoEntry.Text = medicacion.medicamento;
        DosisEntry.Text = medicacion.dosis;
        FrecuenciaEntry.Text = medicacion.frecuencia;
        ObservacionesEditor.Text = medicacion.observaciones;
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
        // Validación de campos obligatorios
        if (string.IsNullOrWhiteSpace(MedicamentoEntry.Text))
        {
            await MostrarErrorAsync("Debes introducir el nombre del medicamento.");
            return;
        }
        if (string.IsNullOrWhiteSpace(DosisEntry.Text))
        {
            await MostrarErrorAsync("Debes introducir la dosis.");
            return;
        }
        if (!double.TryParse(DosisEntry.Text, out var dosis) || dosis <= 0)
        {
            await MostrarErrorAsync("La dosis debe ser un número positivo.");
            return;
        }
        if (string.IsNullOrWhiteSpace(FrecuenciaEntry.Text))
        {
            await MostrarErrorAsync("Debes introducir la frecuencia.");
            return;
        }
        if (!double.TryParse(FrecuenciaEntry.Text, out var frecuencia) || frecuencia <= 0)
        {
            await MostrarErrorAsync("La frecuencia debe ser un número positivo.");
            return;
        }

        var medicacion = new Medicacion
        {
            pacienteId = _pacienteId,
            fechaHora = DateTime.Now,
            medicamento = MedicamentoEntry.Text.Trim(),
            dosis = DosisEntry.Text.Trim(),
            frecuencia = FrecuenciaEntry.Text.Trim(),
            observaciones = ObservacionesEditor.Text?.Trim()
        };

        try
        {
            if (_medicacionEditar != null)
            {
                medicacion.id = _medicacionEditar.id;
                await ApiService.ActualizarMedicacionAsync(medicacion);
            }
            else
            {
                await ApiService.GuardarMedicacionAsync(medicacion);
            }

            await DisplayAlert("Éxito", "Medicación guardada", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar la medicación. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar medicación: {ex}");
        }
    }
}
