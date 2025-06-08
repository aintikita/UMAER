using System.Text.Json;
using System.Text;
using FrontendMAUI.Models;
using FrontendMAUI.Configuración;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FrontendMAUI.Views.Configuracion;

// Página de configuración para la gestión de usuarios
public partial class Configuracion_Page : ContentPage
{
    private readonly HttpClient client = new HttpClient();
    private readonly string baseUrl = $"{Config.BackendUrl}";

    private List<Usuario> usuariosSeleccionados => usuariosFiltrados?.Where(u => u.IsSeleccionado).ToList() ?? new List<Usuario>();
    private ObservableCollection<Usuario> usuariosOriginales;
    private ObservableCollection<Usuario> usuariosFiltrados;

    public Configuracion_Page()
    {
        InitializeComponent();
        CargarUsuarios();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarUsuarios();
    }

    private void BuscarUsuarioBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var filtro = e.NewTextValue?.ToLower() ?? "";

        var filtrados = usuariosOriginales
            .Where(u => u.nombreUsuario.ToLower().Contains(filtro))
            .ToList();

        usuariosFiltrados.Clear();
        foreach (var usuario in filtrados)
            usuariosFiltrados.Add(usuario);
    }

    private async void CargarUsuarios()
    {
        try
        {
            var response = await client.GetAsync($"{baseUrl}/usuarios");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var usuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido) ?? new List<Usuario>();

                foreach (var u in usuarios)
                    u.IsSeleccionado = false;

                usuariosOriginales = new ObservableCollection<Usuario>(usuarios);
                usuariosFiltrados = new ObservableCollection<Usuario>(usuarios);
                UsuariosListView.ItemsSource = usuariosFiltrados;
            }
            else
            {
                await MostrarErrorAsync("No se pudieron cargar los usuarios.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Error de conexión al cargar usuarios.");
            Debug.WriteLine($"[ERROR] {ex}");
        }
    }

    private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }

        var seleccionados = usuariosSeleccionados;
        if (!seleccionados.Any())
        {
            await MostrarErrorAsync("Debes seleccionar al menos un usuario para eliminar.");
            return;
        }

        bool confirmar = await DisplayAlert("Eliminar", $"¿Eliminar a {seleccionados.Count} usuario(s)?", "Sí", "No");
        if (!confirmar) return;

        bool huboError = false;
        foreach (var usuario in seleccionados)
        {
            try
            {
                var response = await client.DeleteAsync($"{baseUrl}/usuario/{usuario.id}");
                if (!response.IsSuccessStatusCode)
                {
                    huboError = true;
                    await MostrarErrorAsync($"No se pudo eliminar a {usuario.nombreUsuario}.");
                }
            }
            catch (Exception ex)
            {
                huboError = true;
                await MostrarErrorAsync($"Error de conexión al eliminar a {usuario.nombreUsuario}.");
                Debug.WriteLine($"[ERROR] {ex}");
            }
        }
        if (!huboError)
            await DisplayAlert("Éxito", "Usuarios eliminados correctamente.", "OK");
        CargarUsuarios();
    }

    private async void OnEditarUsuarioClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original


        }
        var seleccionados = usuariosFiltrados?.Where(u => u.IsSeleccionado).ToList() ?? new List<Usuario>();

        if (seleccionados.Count == 0)
        {
            await MostrarErrorAsync("Debes seleccionar un usuario para editar.");
            return;
        }
        else if (seleccionados.Count > 1)
        {
            await MostrarErrorAsync("Por favor selecciona únicamente un usuario para editar.");
            return;
        }

        var usuarioParaEditar = seleccionados.First();

        string nuevoNombre = await DisplayPromptAsync("Editar Usuario", "Nuevo nombre:", initialValue: usuarioParaEditar.nombreUsuario);

        if (string.IsNullOrWhiteSpace(nuevoNombre))
        {
            await MostrarErrorAsync("El nombre de usuario no puede estar vacío.");
            return;
        }

        var datos = new { nombreUsuario = nuevoNombre };
        var json = JsonSerializer.Serialize(datos);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PutAsync($"{baseUrl}/usuario/{usuarioParaEditar.id}", content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario actualizado correctamente.", "OK");
                CargarUsuarios();
            }
            else
            {
                var msg = await response.Content.ReadAsStringAsync();
                await MostrarErrorAsync($"No se pudo actualizar el usuario: {msg}");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Error de conexión al actualizar el usuario.");
            Debug.WriteLine($"[ERROR] {ex}");
        }
    }

    private async void OnCrearUsuarioClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        string nuevoUsuario = await DisplayPromptAsync("Nuevo Usuario", "Introduce el nombre de usuario:");
        if (string.IsNullOrWhiteSpace(nuevoUsuario))
        {
            await MostrarErrorAsync("El nombre de usuario no puede estar vacío.");
            return;
        }

        string nuevaContrasena = await DisplayPromptAsync("Contraseña", "Introduce la contraseña:");
        if (string.IsNullOrWhiteSpace(nuevaContrasena))
        {
            await MostrarErrorAsync("La contraseña no puede estar vacía.");
            return;
        }

        string claveAdmin = await DisplayPromptAsync("Clave admin", "Introduce la contraseña del administrador:");
        if (string.IsNullOrWhiteSpace(claveAdmin))
        {
            await MostrarErrorAsync("La clave de administrador no puede estar vacía.");
            return;
        }

        var registro = new RegistroRequest
        {
            usuario = nuevoUsuario,
            contrasena = nuevaContrasena,
            adminPassword = claveAdmin
        };

        var json = JsonSerializer.Serialize(registro);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{baseUrl}/registro", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Usuario creado correctamente.", "OK");
                CargarUsuarios();
            }
            else
            {
                var msg = await response.Content.ReadAsStringAsync();
                await MostrarErrorAsync($"No se pudo registrar el usuario: {msg}");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Problema de conexión al crear el usuario.");
            Debug.WriteLine($"[ERROR] {ex}");
        }
    }

    // Método auxiliar para mostrar mensajes de error
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }
}


