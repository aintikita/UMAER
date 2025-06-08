using FrontendMAUI.Services;

namespace FrontendMAUI.Views.Enfermeria;

public partial class DetallePacientePage : ContentPage
{
    public Paciente Paciente { get; set; }

    public DetallePacientePage(Paciente paciente)
    {
        InitializeComponent();

        if (paciente == null)
            throw new ArgumentNullException(nameof(paciente));

        Paciente = paciente;
        BindingContext = Paciente;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void OnVerConstantesClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new ConstantesPage(Paciente.Id.Value));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de constantes.");
            Console.WriteLine($"[ERROR] Ver constantes: {ex}");
        }
    }

    private async void OnVerMedicacionClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new MedicacionPage(Paciente.Id.Value));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de medicación.");
            Console.WriteLine($"[ERROR] Ver medicación: {ex}");
        }
    }

    private async void OnVerDiuresisClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new DiuresisPage(Paciente.Id.Value));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de diuresis.");
            Console.WriteLine($"[ERROR] Ver diuresis: {ex}");
        }
    }

    private async void OnVerBalanceClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new BalancePage(Paciente.Id.Value));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de balance hídrico.");
            Console.WriteLine($"[ERROR] Ver balance: {ex}");
        }
    }

    private async void OnVerAlergiasClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new AlergiasPage(Paciente.Id.Value));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de alergias.");
            Console.WriteLine($"[ERROR] Ver alergias: {ex}");
        }
    }

    private async void OnVerCuidadosClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new CuidadosEnfermeriaPage(Paciente.Id.Value));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de cuidados de enfermería.");
            Console.WriteLine($"[ERROR] Ver cuidados: {ex}");
        }
    }

    private async void OnVerGraficaClicked(object sender, EventArgs e)
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
            if (Paciente.Id.HasValue)
                await Navigation.PushAsync(new GraficaConstantesPage(Paciente));
            else
                await MostrarErrorAsync("Paciente no válido.");
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo abrir la página de gráfica de constantes.");
            Console.WriteLine($"[ERROR] Ver gráfica: {ex}");
        }
    }
}
