using FrontendMAUI.Services;
using System.Collections.ObjectModel;

namespace FrontendMAUI.Views.Enfermeria;

public partial class AlergiasPage : ContentPage
{
    private readonly int _pacienteId;

    public ObservableCollection<Alergia> Alergias { get; set; } = new();

    public AlergiasPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        BindingContext = this;
        CargarAlergias();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarAlergias();  // Recarga la lista al volver a la página
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void CargarAlergias()
    {
        try
        {
            var lista = await ApiService.GetAlergiasPorPacienteAsync(_pacienteId);
            Alergias.Clear();
            foreach (var a in lista)
                Alergias.Add(a);
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la lista de alergias. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar alergias: {ex}");
        }
    }

    private async void OnAgregarAlergiaClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroAlergiasPage(_pacienteId));
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de registro de alergia.");
            Console.WriteLine($"[ERROR] Agregar alergia: {ex}");
        }
    }

    private async void OnEditarAlergiasClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Alergia alergia)
            {
                await Navigation.PushAsync(new RegistroAlergiasPage(alergia));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener la alergia para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edición de alergia.");
            Console.WriteLine($"[ERROR] Editar alergia: {ex}");
        }
    }

    private async void OnEliminarAlergiasClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button btn && btn.CommandParameter is Alergia alergia)
            {
                bool respuesta = await DisplayAlert("Confirmar", "¿Quieres eliminar esta alergia?", "Sí", "No");
                if (respuesta)
                {
                    try
                    {
                        if (alergia.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar la alergia a eliminar.");
                            return;
                        }
                        await ApiService.EliminarAlergiaAsync(alergia.id.Value);
                        Alergias.Remove(alergia);
                        await DisplayAlert("Éxito", "Alergia eliminada correctamente", "OK");
                    }
                    catch (Exception ex2)
                    {
                        await MostrarErrorAsync("No se pudo eliminar la alergia. Inténtalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar alergia: {ex2}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("No se pudo identificar la alergia a eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error inesperado al intentar eliminar la alergia.");
            Console.WriteLine($"[ERROR] OnEliminarAlergiasClicked: {ex}");
        }
    }
}

