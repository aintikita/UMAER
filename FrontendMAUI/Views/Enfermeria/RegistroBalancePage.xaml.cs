using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class RegistroBalancePage : ContentPage
{
    private readonly int _pacienteId;
    private BalanceHidrico _balanceEditar;

    public RegistroBalancePage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        TituloLabel.Text = "Añadir Balance";
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    public RegistroBalancePage(BalanceHidrico balance) : this(balance.pacienteId)
    {
        _balanceEditar = balance;
        TituloLabel.Text = "Editar Balance";

        IngresoEntry.Text = balance.ingreso.ToString();
        EgresoEntry.Text = balance.egreso.ToString();
        ObservacionesEditor.Text = balance.observaciones;
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
        // Validación de campos
        if (string.IsNullOrWhiteSpace(IngresoEntry.Text) && string.IsNullOrWhiteSpace(EgresoEntry.Text))
        {
            await MostrarErrorAsync("Debes introducir al menos un valor de ingreso o egreso.");
            return;
        }

        float ingreso = 0;
        float egreso = 0;

        if (!string.IsNullOrWhiteSpace(IngresoEntry.Text))
        {
            if (!float.TryParse(IngresoEntry.Text, out ingreso) || ingreso < 0)
            {
                await MostrarErrorAsync("El ingreso debe ser un número positivo o cero.");
                return;
            }
        }

        if (!string.IsNullOrWhiteSpace(EgresoEntry.Text))
        {
            if (!float.TryParse(EgresoEntry.Text, out egreso) || egreso < 0)
            {
                await MostrarErrorAsync("El egreso debe ser un número positivo o cero.");
                return;
            }
        }

        // Al menos uno debe ser mayor que cero
        if (ingreso == 0 && egreso == 0)
        {
            await MostrarErrorAsync("El ingreso y el egreso no pueden ser ambos cero.");
            return;
        }

        var balance = new BalanceHidrico
        {
            pacienteId = _pacienteId,
            fechaHora = DateTime.Now,
            ingreso = ingreso,
            egreso = egreso,
            observaciones = ObservacionesEditor.Text?.Trim()
        };

        try
        {
            if (_balanceEditar != null)
            {
                balance.id = _balanceEditar.id;
                await ApiService.ActualizarBalanceAsync(balance);
            }
            else
            {
                await ApiService.GuardarBalanceAsync(balance);
            }

            await DisplayAlert("Éxito", "Registro guardado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo guardar el registro. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Guardar balance hídrico: {ex}");
        }
    }
}

