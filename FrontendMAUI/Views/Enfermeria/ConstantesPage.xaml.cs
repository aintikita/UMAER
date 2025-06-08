using FrontendMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FrontendMAUI.Views.Enfermeria;

public partial class ConstantesPage : ContentPage
{
    private readonly int _pacienteId;

    public ObservableCollection<Constante> Constantes { get; set; } = new();

    public ConstantesPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        BindingContext = this;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        CargarConstantes();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarConstantes();  // Recarga la lista al volver a la página
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void CargarConstantes()
    {
        try
        {
            var datos = await ApiService.GetConstantesPorPacienteAsync(_pacienteId);
            Constantes.Clear();
            if (datos != null)
            {
                foreach (var c in datos)
                    Constantes.Add(c);
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la lista de constantes. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar constantes: {ex}");
        }
    }

    private async void OnAgregarConstanteClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroConstantesPage(_pacienteId));
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de registro de constante.");
            Console.WriteLine($"[ERROR] Agregar constante: {ex}");
        }
    }

    private async void OnEditarConstanteClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Constante constante)
            {
                await Navigation.PushAsync(new RegistroConstantesPage(constante));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener la constante para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edición de constante.");
            Console.WriteLine($"[ERROR] Editar constante: {ex}");
        }
    }

    private async void OnEliminarConstanteClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Constante constante)
            {
                bool respuesta = await DisplayAlert("Confirmar", "¿Quieres eliminar esta constante?", "Sí", "No");
                if (respuesta)
                {
                    try
                    {
                        if (constante.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar la constante a eliminar.");
                            return;
                        }
                        await ApiService.EliminarConstanteAsync(constante.id.Value);
                        Constantes.Remove(constante);
                        await DisplayAlert("Éxito", "Constante eliminada correctamente", "OK");
                    }
                    catch (Exception ex2)
                    {
                        await MostrarErrorAsync("No se pudo eliminar la constante. Inténtalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar constante: {ex2}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("No se pudo identificar la constante a eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error inesperado al intentar eliminar la constante.");
            Console.WriteLine($"[ERROR] OnEliminarConstanteClicked: {ex}");
        }
    }
}
