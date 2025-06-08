using FrontendMAUI.Services;
using System.Collections.ObjectModel;

namespace FrontendMAUI.Views.Enfermeria;

public partial class CuidadosEnfermeriaPage : ContentPage
{
    private readonly int _pacienteId;

    public ObservableCollection<CuidadoEnfermeria> Cuidados { get; set; } = new();

    public CuidadosEnfermeriaPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        BindingContext = this;
        CargarCuidados();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarCuidados();  // Recarga la lista al volver a la página
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void CargarCuidados()
    {
        try
        {
            var lista = await ApiService.GetCuidadosPorPacienteAsync(_pacienteId);
            Cuidados.Clear();
            foreach (var c in lista)
                Cuidados.Add(c);
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la lista de cuidados. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar cuidados: {ex}");
        }
    }

    private async void OnAgregarCuidadoClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroCuidadosEnfermeriaPage(_pacienteId));
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de registro de cuidado.");
            Console.WriteLine($"[ERROR] Agregar cuidado: {ex}");
        }
    }

    private async void OnEditarCuidadosClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is CuidadoEnfermeria cuidado)
            {
                await Navigation.PushAsync(new RegistroCuidadosEnfermeriaPage(cuidado));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener el cuidado para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edición de cuidado.");
            Console.WriteLine($"[ERROR] Editar cuidado: {ex}");
        }
    }

    private async void OnEliminarCuidadosClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is CuidadoEnfermeria cuidado)
            {
                bool respuesta = await DisplayAlert("Confirmar", "¿Quieres eliminar este cuidado?", "Sí", "No");
                if (respuesta)
                {
                    try
                    {
                        if (cuidado.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar el cuidado a eliminar.");
                            return;
                        }
                        await ApiService.EliminarCuidadoAsync(cuidado.id.Value);
                        Cuidados.Remove(cuidado);
                        await DisplayAlert("Éxito", "Cuidado eliminado correctamente", "OK");
                    }
                    catch (Exception ex2)
                    {
                        await MostrarErrorAsync("No se pudo eliminar el cuidado. Inténtalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar cuidado: {ex2}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("No se pudo identificar el cuidado a eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error inesperado al intentar eliminar el cuidado.");
            Console.WriteLine($"[ERROR] OnEliminarCuidadosClicked: {ex}");
        }
    }
}

