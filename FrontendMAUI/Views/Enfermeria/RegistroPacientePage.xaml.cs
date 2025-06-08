using FrontendMAUI;
using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroPacientePage : ContentPage
{
    private Paciente _pacienteEditar;

    public RegistroPacientePage()
    {
        InitializeComponent();
        TituloLabel.Text = "Añadir Paciente";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    public RegistroPacientePage(Paciente paciente) : this()
    {
        _pacienteEditar = paciente;
        TituloLabel.Text = "Editar Paciente";

        NombreEntry.Text = paciente.Nombre;
        HabitacionEntry.Text = paciente.Habitacion;
        UnidadEntry.Text = paciente.Unidad;
        EdadEntry.Text = paciente.Edad.ToString();
        PesoEntry.Text = paciente.Peso.ToString();
        AlturaEntry.Text = paciente.Altura.ToString();
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private bool ValidarCampos(
        out string nombre,
        out string habitacion,
        out string unidad,
        out int edad,
        out float peso,
        out float altura)
    {
        nombre = NombreEntry.Text?.Trim() ?? "";
        habitacion = HabitacionEntry.Text?.Trim() ?? "";
        unidad = UnidadEntry.Text?.Trim() ?? "";
        edad = 0;
        peso = 0;
        altura = 0;

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarErrorAsync("Debes introducir un nombre.").ConfigureAwait(false);
            return false;
        }
        if (string.IsNullOrWhiteSpace(habitacion))
        {
            MostrarErrorAsync("Debes introducir la habitación.").ConfigureAwait(false);
            return false;
        }
        if (string.IsNullOrWhiteSpace(unidad))
        {
            MostrarErrorAsync("Debes introducir la unidad.").ConfigureAwait(false);
            return false;
        }
        if (string.IsNullOrWhiteSpace(EdadEntry.Text) ||
            !int.TryParse(EdadEntry.Text, out edad) || edad <= 0)
        {
            MostrarErrorAsync("La edad debe ser un número entero positivo.").ConfigureAwait(false);
            return false;
        }
        if (string.IsNullOrWhiteSpace(PesoEntry.Text) ||
            !float.TryParse(PesoEntry.Text, out peso) || peso <= 0)
        {
            MostrarErrorAsync("El peso debe ser un número positivo.").ConfigureAwait(false);
            return false;
        }
        if (string.IsNullOrWhiteSpace(AlturaEntry.Text) ||
            !float.TryParse(AlturaEntry.Text, out altura) || altura <= 0)
        {
            MostrarErrorAsync("La altura debe ser un número positivo.").ConfigureAwait(false);
            return false;
        }
        return true;
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
        if (!ValidarCampos(out var nombre, out var habitacion, out var unidad, out var edad, out var peso, out var altura))
            return;

        var paciente = new Paciente
        {
            Id = _pacienteEditar?.Id,
            Nombre = nombre,
            Habitacion = habitacion,
            Unidad = unidad,
            Edad = edad,
            Peso = peso,
            Altura = altura,
        };

        try
        {
            if (_pacienteEditar != null)
            {
                await ApiService.ActualizarPacienteAsync(paciente);
                await DisplayAlert("Éxito", $"Paciente {paciente.Nombre} actualizado.", "OK");
            }
            else
            {
                var pacienteGuardado = await ApiService.GuardarPacienteAsync(paciente);
                await DisplayAlert("Éxito", $"Paciente {pacienteGuardado.Nombre} registrado con ID {pacienteGuardado.Id}.", "OK");
            }
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar el paciente. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar paciente: {ex}");
        }
    }
}


