using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroAlergiasPage : ContentPage
{
    private readonly int _pacienteId;
    private Alergia _alergiaEditar;

    public RegistroAlergiasPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        TituloLabel.Text = "Añadir Alergia";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    public RegistroAlergiasPage(Alergia alergia) : this(alergia.pacienteId)
    {
        _alergiaEditar = alergia;
        TituloLabel.Text = "Editar Alergia";

        NombreAlergiaEntry.Text = alergia.alergia;
        DescripcionEditor.Text = alergia.descripcion;
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
        // Validación de campo obligatorio
        if (string.IsNullOrWhiteSpace(NombreAlergiaEntry.Text))
        {
            await MostrarErrorAsync("Debes introducir el nombre de la alergia.");
            return;
        }

        var alergia = new Alergia
        {
            pacienteId = _pacienteId,
            alergia = NombreAlergiaEntry.Text.Trim(),
            descripcion = DescripcionEditor.Text?.Trim()
        };

        try
        {
            if (_alergiaEditar != null)
            {
                alergia.id = _alergiaEditar.id;
                await ApiService.ActualizarAlergiaAsync(alergia);
            }
            else
            {
                await ApiService.GuardarAlergiaAsync(alergia);
            }

            await DisplayAlert("Éxito", "Alergia guardada", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar la alergia. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar alergia: {ex}");
        }
    }
}


