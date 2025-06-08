using FrontendMAUI.Services;
using System.Collections.ObjectModel;

namespace FrontendMAUI.Views.Enfermeria;

public partial class MedicacionPage : ContentPage
{
    private readonly int _pacienteId;

    public ObservableCollection<Medicacion> Medicaciones { get; set; } = new();

    public MedicacionPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        BindingContext = this;
        CargarMedicaciones();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarMedicaciones();  // Recarga la lista al volver a la página
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void CargarMedicaciones()
    {
        try
        {
            var lista = await ApiService.GetMedicacionPorPacienteAsync(_pacienteId);
            Medicaciones.Clear();
            foreach (var m in lista)
                Medicaciones.Add(m);
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la medicación. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar medicación: {ex}");
        }
    }

    private async void OnAgregarMedicacionClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroMedicacionPage(_pacienteId));
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de medicación.");
            Console.WriteLine($"[ERROR] Agregar medicación: {ex}");
        }
    }

    private async void OnEditarMedicacionClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Medicacion medicacion)
            {
                await Navigation.PushAsync(new RegistroMedicacionPage(medicacion));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener la medicación para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edición.");
            Console.WriteLine($"[ERROR] Editar medicación: {ex}");
        }
    }

    private async void OnEliminarMedicacionClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Medicacion medicacion)
            {
                bool respuesta = await DisplayAlert("Confirmar", "¿Quieres eliminar esta medicación?", "Sí", "No");
                if (respuesta)
                {
                    try
                    {
                        if (medicacion.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar la medicación a eliminar.");
                            return;
                        }
                        await ApiService.EliminarMedicacionAsync(medicacion.id.Value);
                        Medicaciones.Remove(medicacion);
                        await DisplayAlert("Éxito", "Medicación eliminada correctamente", "OK");
                    }
                    catch (Exception ex)
                    {
                        await MostrarErrorAsync("No se pudo eliminar la medicación. Inténtalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar medicación: {ex}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("No se pudo identificar la medicación a eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error inesperado al intentar eliminar la medicación.");
            Console.WriteLine($"[ERROR] OnEliminarMedicacionClicked: {ex}");
        }
    }
}
