using System;
using System.Collections.Generic;
using System.Net.Http;
using FrontendMAUI.Configuración;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace FrontendMAUI.Views.Auth
{
    // Página de registro de nuevos usuarios
    public partial class RegisterPage : ContentPage
    {
        private readonly HttpClient cliente = new HttpClient();

        // Constructor: inicializa la vista y oculta la barra de navegación
        public RegisterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            lblMensaje.Text = string.Empty;
        }

        // Evento que se ejecuta al pulsar el botón de registrar
        private async void OnRegistrarClicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            if (boton != null)
            {
                var colorOriginal = boton.BackgroundColor; // Guarda el color original

                boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
                await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
                boton.BackgroundColor = colorOriginal; // Vuelve al color original
            }

            // Limpiar mensajes previos
            lblMensaje.Text = string.Empty;

            // Obtiene y limpia los valores de los campos del formulario
            string nuevoUsuario = txtNuevoUsuario.Text?.Trim();
            string nuevaPassword = txtNuevaPassword.Text?.Trim();
            string confirmarPassword = txtConfirmarPassword.Text?.Trim();
            string claveAdmin = txtClaveAdmin.Text?.Trim();

            // Validación: todos los campos son obligatorios
            if (string.IsNullOrEmpty(nuevoUsuario) ||
                string.IsNullOrEmpty(nuevaPassword) ||
                string.IsNullOrEmpty(confirmarPassword) ||
                string.IsNullOrEmpty(claveAdmin))
            {
                lblMensaje.Text = "⚠️ Completa todos los campos.";
                return;
            }

            // Validación: las contraseñas deben coincidir
            if (nuevaPassword != confirmarPassword)
            {
                lblMensaje.Text = "⚠️ Las contraseñas no coinciden.";
                return;
            }

            try
            {
                // Prepara los datos para enviar al backend
                var datos = new Dictionary<string, string>
                {
                    { "usuario", nuevoUsuario },
                    { "contrasena", nuevaPassword },
                    { "adminPassword", claveAdmin } // Se envía la clave de admin también
                };

                var json = JsonSerializer.Serialize(datos);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                // Realiza la petición POST al endpoint de registro
                var respuesta = await cliente.PostAsync($"{Config.BackendUrl}/registro", contenido);
                var cuerpo = await respuesta.Content.ReadAsStringAsync();

                if (respuesta.IsSuccessStatusCode)
                {
                    // Registro exitoso: muestra alerta y vuelve al login
                    await DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    // Error devuelto por el backend, mostrar mensaje
                    lblMensaje.Text = $"❌ Error: {cuerpo}";
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Error específico de conexión HTTP
                lblMensaje.Text = $"❌ Error de conexión: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                // Cualquier otra excepción inesperada
                lblMensaje.Text = $"❌ Error inesperado: {ex.Message}";
            }
        }
    }
}

