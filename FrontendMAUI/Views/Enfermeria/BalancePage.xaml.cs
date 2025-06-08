using FrontendMAUI.Services;
using System.Collections.ObjectModel;

namespace FrontendMAUI.Views.Enfermeria;

public partial class BalancePage : ContentPage
{
    private readonly int _pacienteId;

    public ObservableCollection<BalanceHidrico> Balances { get; set; } = new();

    public BalancePage(int pacienteId)
    {
        InitializeComponent();
        _pacienteId = pacienteId;
        BindingContext = this;
        CargarBalances();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarBalances();  // Recarga la lista al volver a la página
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void CargarBalances()
    {
        try
        {
            var lista = await ApiService.GetBalancePorPacienteAsync(_pacienteId);
            Balances.Clear();
            foreach (var b in lista)
                Balances.Add(b);
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la lista de balances. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar balances: {ex}");
        }
    }

    private async void OnAgregarBalanceClicked(object sender, EventArgs e)
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
            await Navigation.PushAsync(new RegistroBalancePage(_pacienteId));
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de registro de balance.");
            Console.WriteLine($"[ERROR] Agregar balance: {ex}");
        }
    }

    private async void OnEditarBalanceClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is BalanceHidrico balance)
            {
                await Navigation.PushAsync(new RegistroBalancePage(balance));
            }
            else
            {
                await MostrarErrorAsync("No se pudo obtener el registro para editar.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir el formulario de edición de balance.");
            Console.WriteLine($"[ERROR] Editar balance: {ex}");
        }
    }

    private async void OnEliminarBalanceClicked(object sender, EventArgs e)
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
            if (sender is Button btn && btn.CommandParameter is BalanceHidrico balance)
            {
                bool respuesta = await DisplayAlert("Confirmar", "¿Quieres eliminar este registro?", "Sí", "No");
                if (respuesta)
                {
                    try
                    {
                        if (balance.id == null)
                        {
                            await MostrarErrorAsync("No se pudo identificar el registro a eliminar.");
                            return;
                        }
                        await ApiService.EliminarBalanceAsync(balance.id.Value);
                        Balances.Remove(balance);
                        await DisplayAlert("Éxito", "Registro eliminado correctamente", "OK");
                    }
                    catch (Exception ex2)
                    {
                        await MostrarErrorAsync("No se pudo eliminar el registro. Inténtalo de nuevo.");
                        Console.WriteLine($"[ERROR] Eliminar balance: {ex2}");
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
            Console.WriteLine($"[ERROR] OnEliminarBalanceClicked: {ex}");
        }
    }
}

