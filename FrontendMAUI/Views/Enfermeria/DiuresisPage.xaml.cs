using FrontendMAUI.Services;
using System.Collections.ObjectModel;

namespace FrontendMAUI.Views.Enfermeria;

public partial class DiuresisPage : ContentPage
{
    private readonly int _pacienteId;

    public ObservableCollection<Diuresis> Diuresis { get; set; } = new();

    public DiuresisPage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        BindingContext = this;
        CargarDiuresis();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarDiuresis();  // Recarga la lista al volver a la página
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void CargarDiuresis()
    {
        try
        {
            var lista = await ApiService.GetDiuresisPorPacienteAsync(_pacienteId);
            Diuresis.Clear();
            foreach (var d in lista)
                Diuresis.Add(d);
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la lista de diuresis. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar diuresis: {ex}");
        }
    }

    private async void OnAgregarDiuresisClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroDiuresisPage(_pacienteId));
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de registro de diuresis.");
            Console.WriteLine($"[ERROR] Agregar diuresis: {ex}");
        }
    }

    private async void OnEditarDiuresisClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Diuresis diuresis)
            {
                await Navigation.PushAsync(new RegistroDiuresisPage(diuresis));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener el registro para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edición de diuresis.");
            Console.WriteLine($"[ERROR] Editar diuresis: {ex}");
        }
    }

    private async void OnEliminarDiuresisClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is Diuresis diuresis)
            {
                bool respuesta = await DisplayAlert("Confirmar", "¿Quieres eliminar este registro?", "Sí", "No");
                if (respuesta)
                {
                    try
                    {
                        if (diuresis.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar el registro a eliminar.");
                            return;
                        }
                        await ApiService.EliminarDiuresisAsync(diuresis.id.Value);
                        Diuresis.Remove(diuresis);
                        await DisplayAlert("Éxito", "Registro eliminado correctamente", "OK");
                    }
                    catch (Exception ex2)
                    {
                        await MostrarErrorAsync("No se pudo eliminar el registro. Inténtalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar diuresis: {ex2}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("No se pudo identificar el registro a eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error inesperado al intentar eliminar el registro.");
            Console.WriteLine($"[ERROR] OnEliminarDiuresisClicked: {ex}");
        }
    }
}

