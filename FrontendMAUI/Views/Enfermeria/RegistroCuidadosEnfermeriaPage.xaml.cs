using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroCuidadosEnfermeriaPage : ContentPage
{
    private readonly int _pacienteId;
    private CuidadoEnfermeria _cuidadoEditar;

    public RegistroCuidadosEnfermeriaPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        TituloLabel.Text = "Añadir Cuidado";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    public RegistroCuidadosEnfermeriaPage(CuidadoEnfermeria cuidado) : this(cuidado.pacienteId)
    {
        _cuidadoEditar = cuidado;
        TituloLabel.Text = "Editar Cuidado";
        DescripcionEditor.Text = cuidado.descripcion;
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
        if (string.IsNullOrWhiteSpace(DescripcionEditor.Text))
        {
            await MostrarErrorAsync("Debes introducir una descripción del cuidado.");
            return;
        }

        var cuidado = new CuidadoEnfermeria
        {
            pacienteId = _pacienteId,
            fechaHora = DateTime.Now,
            descripcion = DescripcionEditor.Text.Trim()
        };

        try
        {
            if (_cuidadoEditar != null)
            {
                cuidado.id = _cuidadoEditar.id;
                await ApiService.ActualizarCuidadoAsync(cuidado);
            }
            else
            {
                await ApiService.GuardarCuidadoAsync(cuidado);
            }

            await DisplayAlert("Éxito", "Cuidado guardado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar el cuidado. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar cuidado: {ex}");
        }
    }
}


