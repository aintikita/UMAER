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
        CargarMedicaciones();  // Recarga la lista al volver a la p�gina
    }

    // M�todo auxiliar para mostrar mensajes de error amigables
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
            await MostrarErrorAsync("No se pudo cargar la medicaci�n. Int�ntalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar medicaci�n: {ex}");
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
            await MostrarErrorAsync("No se pudo abrir el formulario de medicaci�n.");
            Console.WriteLine($"[ERROR] Agregar medicaci�n: {ex}");
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
                await MostrarErrorAsync("No se pudo obtener la medicaci�n para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edici�n.");
            Console.WriteLine($"[ERROR] Editar medicaci�n: {ex}");
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
                bool respuesta = await DisplayAlert("Confirmar", "�Quieres eliminar esta medicaci�n?", "S�", "No");
                if (respuesta)
                {
                    try
                    {
                        if (medicacion.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar la medicaci�n a eliminar.");
                            return;
                        }
                        await ApiService.EliminarMedicacionAsync(medicacion.id.Value);
                        Medicaciones.Remove(medicacion);
                        await DisplayAlert("�xito", "Medicaci�n eliminada correctamente", "OK");
                    }
                    catch (Exception ex)
                    {
                        await MostrarErrorAsync("No se pudo eliminar la medicaci�n. Int�ntalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar medicaci�n: {ex}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("No se pudo identificar la medicaci�n a eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurri� un error inesperado al intentar eliminar la medicaci�n.");
            Console.WriteLine($"[ERROR] OnEliminarMedicacionClicked: {ex}");
        }
    }
}
