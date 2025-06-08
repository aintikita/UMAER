using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    // M�todo auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPacientesAsync();
    }

    private async Task CargarPacientesAsync()
    {
        try
        {
            var pacientes = await ApiService.GetPacientesAsync();
            PacientesListView.ItemsSource = pacientes;
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la lista de pacientes. Int�ntalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar pacientes: {ex}");
        }
    }

    private async void OnAddPacienteClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroPacientePage());
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de registro de paciente.");
            Console.WriteLine($"[ERROR] Agregar paciente: {ex}");
        }
    }

    private async void OnVerPacienteClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.BindingContext is Paciente paciente)
            {
                await Navigation.PushAsync(new DetallePacientePage(paciente));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener el paciente para ver detalles.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la p�gina de detalles del paciente.");
            Console.WriteLine($"[ERROR] Ver paciente: {ex}");
        }
    }

    private async void OnEliminarPacienteClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.BindingContext is Paciente paciente && paciente.Id.HasValue)
            {
                bool confirmar = await DisplayAlert("Confirmar eliminaci�n", $"�Eliminar a {paciente.Nombre}?", "S�", "No");
                if (confirmar)
                {
                    try
                    {
                        bool exito = await ApiService.EliminarPacienteAsync(paciente.Id.Value);
                        if (exito)
                        {
                            await DisplayAlert("Paciente eliminado", "El paciente ha sido eliminado.", "OK");
                            await CargarPacientesAsync();
                        }
                        else
                        {
                            await MostrarErrorAsync("No se pudo eliminar el paciente.");
                        }
                    }
                    catch (Exception ex2)
                    {
                        await MostrarErrorAsync("Error al eliminar paciente. Int�ntalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar paciente: {ex2}");
                    }
                }
            }
            else
            {
                await MostrarErrorAsync("Paciente inv�lido para eliminar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurri� un error inesperado al intentar eliminar el paciente.");
            Console.WriteLine($"[ERROR] OnEliminarPacienteClicked: {ex}");
        }
    }

    private async void OnEditarPacienteClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.BindingContext is Paciente paciente)
            {
                await Navigation.PushAsync(new RegistroPacientePage(paciente));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener el paciente para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edici�n de paciente.");
            Console.WriteLine($"[ERROR] Editar paciente: {ex}");
        }
    }
}

