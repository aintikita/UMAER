using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroDiuresisPage : ContentPage
{
    private readonly int _pacienteId;
    private Diuresis _diuresisEditar;

    public RegistroDiuresisPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        TituloLabel.Text = "Añadir Diuresis";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    public RegistroDiuresisPage(Diuresis diuresis) : this(diuresis.pacienteId)
    {
        _diuresisEditar = diuresis;
        TituloLabel.Text = "Editar Diuresis";
        CantidadEntry.Text = diuresis.cantidad.ToString();
        ObservacionesEditor.Text = diuresis.observaciones;
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
        // Validación de campo cantidad
        if (string.IsNullOrWhiteSpace(CantidadEntry.Text))
        {
            await MostrarErrorAsync("Debes introducir la cantidad.");
            return;
        }
        if (!float.TryParse(CantidadEntry.Text, out var cantidad) || cantidad <= 0)
        {
            await MostrarErrorAsync("La cantidad debe ser un número positivo.");
            return;
        }

        var diuresis = new Diuresis
        {
            pacienteId = _pacienteId,
            fechaHora = DateTime.Now,
            cantidad = cantidad,
            observaciones = ObservacionesEditor.Text?.Trim()
        };

        try
        {
            if (_diuresisEditar != null)
            {
                diuresis.id = _diuresisEditar.id;
                await ApiService.ActualizarDiuresisAsync(diuresis);
            }
            else
            {
                await ApiService.GuardarDiuresisAsync(diuresis);
            }

            await DisplayAlert("Éxito", "Registro guardado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar el registro. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar diuresis: {ex}");
        }
    }
}

