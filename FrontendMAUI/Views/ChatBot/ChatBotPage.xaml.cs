using FrontendMAUI.Services;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using FrontendMAUI.Configuración;
using System.Text;

namespace FrontendMAUI.Views.ChatBot;

// Página de chat para interactuar con el chatbot del backend
public partial class ChatBotPage : ContentPage
{
    // Cliente HTTP para enviar preguntas al backend
    private readonly HttpClient _http = new();
    // URL del endpoint del chatbot
    private readonly string backendUrl = $"{Config.BackendUrl}/pdf/preguntar";

    // Constructor: inicializa la vista
    public ChatBotPage()
    {
        InitializeComponent();
    }

    // Evento que se ejecuta al pulsar el botón de enviar
    // Envía la pregunta al backend y muestra la respuesta
    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor;
            boton.BackgroundColor = Color.FromArgb("#7cc191");
            await Task.Delay(300);
            boton.BackgroundColor = colorOriginal;
        }

        var pregunta = PreguntaEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(pregunta))
            return;

        MostrarMensaje(pregunta, esUsuario: true);
        PreguntaEntry.Text = "";

        // 👉 Mostrar "Pensando..." mientras se genera respuesta
        var cargando = MostrarMensaje("⏳ Pensando...", esUsuario: false);

        try
        {
            var payload = new { pregunta };
            var json = JsonSerializer.Serialize(payload);

            var response = await _http.PostAsync(backendUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));

            MensajesLayout.Children.Remove(cargando); // Quitar mensaje de espera

            if (!response.IsSuccessStatusCode)
            {
                MostrarMensaje("⚠️ Error del servidor", esUsuario: false);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(content);

            if (doc.RootElement.TryGetProperty("respuesta", out var respuesta))
            {
                MostrarMensaje(respuesta.GetString() ?? "Respuesta vacía", esUsuario: false);
            }
            else
            {
                MostrarMensaje("❌ Respuesta no válida.", esUsuario: false);
            }
        }
        catch (Exception ex)
        {
            MensajesLayout.Children.Remove(cargando); // Quitar mensaje de espera
            MostrarMensaje("❌ Error: " + ex.Message, esUsuario: false);
        }
    }

    // Muestra un mensaje en el chat, diferenciando usuario y bot
    private View MostrarMensaje(string texto, bool esUsuario)
    {
        var burbuja = new Frame
        {
            CornerRadius = 12,
            Padding = 10,
            Margin = new Thickness(5, 2),
            BackgroundColor = esUsuario ? Colors.LightSteelBlue : Colors.LightGray,
            HorizontalOptions = esUsuario ? LayoutOptions.End : LayoutOptions.Start,
            Content = new Label
            {
                Text = texto,
                TextColor = Colors.Black,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Start
            }
        };

        MensajesLayout.Children.Add(burbuja);
        return burbuja; // devolvemos para poder quitarlo
    }
}




