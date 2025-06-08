using System.Net.Http;
using System.Text;
using System.Text.Json;
using FrontendMAUI.Configuración;

namespace FrontendMAUI.Views.Auth;

public partial class OlvidarContrasenaPage : ContentPage
{
    private readonly HttpClient client = new();
    private readonly string baseUrl = $"{Config.BackendUrl}"; // Ajusta si cambia

    public OlvidarContrasenaPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnRestablecerClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original
        }

        string usuario = UsuarioEntry.Text?.Trim();
        string nuevaContrasena = NuevaContrasenaEntry.Text;
        string confirmar = ConfirmarContrasenaEntry.Text;

        MensajeLabel.Text = "";

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(nuevaContrasena) || string.IsNullOrWhiteSpace(confirmar))
        {
            MensajeLabel.Text = "Todos los campos son obligatorios.";
            return;
        }

        if (nuevaContrasena != confirmar)
        {
            MensajeLabel.Text = "Las contraseñas no coinciden.";
            return;
        }

        try
        {
            var datos = new
            {
                nombreUsuario = usuario,
                nuevaContrasena = nuevaContrasena
            };

            var json = JsonSerializer.Serialize(datos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/usuarios/reset-password", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Contraseña actualizada correctamente.", "OK");
                await Navigation.PopAsync(); // volver al login
            }
            else
            {
                var mensaje = await response.Content.ReadAsStringAsync();
                MensajeLabel.Text = mensaje; // Backend puede enviar mensaje como: "Usuario no encontrado" o "Misma contraseña"
            }
        }
        catch (Exception ex)
        {
            MensajeLabel.Text = $"Error de conexión: {ex.Message}";
        }

        
    }
}
