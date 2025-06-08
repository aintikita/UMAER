// Views/Mochilas/MochilaPage.xaml.cs
using FrontendMAUI.Models;
using FrontendMAUI.Services;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using FrontendMAUI.Configuración;

namespace FrontendMAUI.Views.Mochilas;

public partial class MochilaPage : ContentPage
{
    private Stack<string> rutaStack = new();
    private readonly HttpClient _http = new();
    public static bool EsAdmin { get; set; } = false;

    public MochilaPage()
    {
        InitializeComponent();
        CargarContenido("");
        AñadirArchivoBtn.IsVisible = App.EsAdmin;
        CrearCarpetaBtn.IsVisible = App.EsAdmin;
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    private async void ListaMochilas_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is ArchivoDocumento item)
        {
            try
            {
                if (item.EsCarpeta)
                {
                    string rutaRelativa = item.Ruta.Replace("mochilas/", "");
                    rutaStack.Push(rutaRelativa);
                    CargarContenido(rutaRelativa);
                }
                else
                {
                    string archivoNombre = item.Nombre.EndsWith(".pdf") ? item.Nombre : item.Nombre + ".pdf";
                    var encodedRuta = item.Ruta.Replace(" ", "%20");
                    var url = $"{Config.BackendUrl}/mochilas/abrir?archivo={encodedRuta}";

                    string localPath = string.Empty;
                    try
                    {
                        localPath = await ArchivoHelper.DescargarPdfLocalAsync(archivoNombre, url);
                    }
                    catch (Exception ex)
                    {
                        await MostrarErrorAsync("No se pudo descargar el archivo PDF. Verifica tu conexión o el archivo.");
                        Debug.WriteLine($"[ERROR] Descarga PDF: {ex}");
                        return;
                    }

                    if (!string.IsNullOrEmpty(localPath) && File.Exists(localPath))
                    {
                        try
                        {
                            await Navigation.PushAsync(new PdfViewerPage(localPath, item.Nombre));
                        }
                        catch (Exception ex)
                        {
                            await MostrarErrorAsync("No se pudo abrir el visor de PDF.");
                            Debug.WriteLine($"[ERROR] Abrir visor PDF: {ex}");
                        }
                    }
                    else
                    {
                        await MostrarErrorAsync("No se pudo descargar el archivo PDF.");
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("Ocurrió un error inesperado al abrir el archivo o carpeta.");
                Debug.WriteLine($"[ERROR] ItemTapped: {ex}");
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
    private async void CrearCarpetaBtn_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        // Solicita al usuario el nombre de la nueva carpeta
        string nuevaCarpeta = await DisplayPromptAsync("Nueva Carpeta", "Introduce el nombre de la carpeta:");

        // Comprueba si el usuario ingresó un nombre válido
        if (string.IsNullOrWhiteSpace(nuevaCarpeta)) return;

        // Preparar petición al backend
        string rutaActual = rutaStack.Count > 0 ? rutaStack.Peek() : "";
        var carpetaBackend = string.IsNullOrEmpty(rutaActual) ? "po" : Path.Combine("po", rutaActual).Replace("\\", "/");

        var json = JsonSerializer.Serialize(new { carpetaPadre = carpetaBackend, nombreNuevaCarpeta = nuevaCarpeta });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            // Enviar petición HTTP al backend para crear carpeta
            var response = await _http.PostAsync($"{Config.BackendUrl}/carpeta/crear", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", $"Carpeta '{nuevaCarpeta}' creada.", "OK");
                CargarContenido(rutaActual); // Actualizar lista
            }
            else
            {
                var msg = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo crear la carpeta: {msg}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Problema de conexión: {ex.Message}", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AñadirArchivoBtn.IsVisible = App.EsAdmin;
        CrearCarpetaBtn.IsVisible = App.EsAdmin;
    }

    private async void CargarContenido(string ruta)
    {
        try
        {
            var contenido = await MochilaService.ObtenerContenidoAsync(ruta);
            ListaMochilas.ItemsSource = contenido;
            VolverBtn.IsVisible = rutaStack.Count > 0;
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudo cargar la carpeta. Verifica tu conexión.");
            Debug.WriteLine($"[ERROR] CargarContenido: {ex}");
        }
    }

    private void VolverBtn_Clicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#a2a5b7"); // Cambia al color al hacer click
            Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        try
        {
            if (rutaStack.Count > 0)
            {
                rutaStack.Pop();
                var anterior = rutaStack.Count > 0 ? rutaStack.Peek() : "";
                CargarContenido(anterior);
            }
        }
        catch (Exception ex)
        {
            MostrarErrorAsync("No se pudo volver a la carpeta anterior.").ConfigureAwait(false);
            Debug.WriteLine($"[ERROR] VolverBtn_Clicked: {ex}");
        }
    }

    private async void ListaMochilas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ArchivoDocumento item)
        {
            try
            {
                if (item.EsCarpeta)
                {
                    string rutaRelativa = item.Ruta.Replace("mochilas/", "");
                    rutaStack.Push(rutaRelativa);
                    CargarContenido(rutaRelativa);
                }
                else
                {
                    string archivoNombre = item.Nombre.EndsWith(".pdf") ? item.Nombre : item.Nombre + ".pdf";
                    var encodedRuta = item.Ruta.Replace(" ", "%20");
                    var url = $"{Config.BackendUrl}/mochilas/abrir?archivo={encodedRuta}";

                    string localPath = string.Empty;
                    try
                    {
                        localPath = await ArchivoHelper.DescargarPdfLocalAsync(archivoNombre, url);
                    }
                    catch (Exception ex)
                    {
                        await MostrarErrorAsync("No se pudo descargar el archivo PDF. Verifica tu conexión o el archivo.");
                        Debug.WriteLine($"[ERROR] Descarga PDF: {ex}");
                        return;
                    }

                    if (!string.IsNullOrEmpty(localPath) && File.Exists(localPath))
                    {
                        try
                        {
                            await Navigation.PushAsync(new PdfViewerPage(localPath, item.Nombre));
                        }
                        catch (Exception ex)
                        {
                            await MostrarErrorAsync("No se pudo abrir el visor de PDF.");
                            Debug.WriteLine($"[ERROR] Abrir visor PDF: {ex}");
                        }
                    }
                    else
                    {
                        await MostrarErrorAsync("No se pudo descargar el archivo PDF.");
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("Ocurrió un error inesperado al abrir el archivo o carpeta.");
                Debug.WriteLine($"[ERROR] SelectionChanged: {ex}");
            }

            ListaMochilas.SelectedItem = null;
        }
    }

    public static implicit operator View(MochilaPage v)
    {
        throw new NotImplementedException();
    }

    private async void AñadirArchivoBtn_Clicked(object sender, EventArgs e)
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
            var resultado = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Pdf,
                PickerTitle = "Selecciona un archivo PDF para subir"
            });

            if (resultado == null)
                return; // Canceló la selección

            string rutaArchivo = resultado.FullPath;
            string nombreArchivo = resultado.FileName;

            if (string.IsNullOrWhiteSpace(rutaArchivo) || string.IsNullOrWhiteSpace(nombreArchivo))
            {
                await MostrarErrorAsync("Archivo no válido.");
                return;
            }

            string rutaActual = rutaStack.Count > 0 ? rutaStack.Peek() : "";
            string carpetaBackend = string.IsNullOrEmpty(rutaActual) ? "mochilas" : Path.Combine("mochilas", rutaActual).Replace("\\", "/");

            bool exito = false;
            try
            {
                exito = await SubirArchivoPdf(carpetaBackend, rutaArchivo, nombreArchivo);
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("No se pudo subir el archivo. Verifica tu conexión o el archivo.");
                Debug.WriteLine($"[ERROR] SubirArchivoPdf: {ex}");
                return;
            }

            if (exito)
            {
                await DisplayAlert("Éxito", $"Archivo '{nombreArchivo}' subido a '{carpetaBackend}'", "OK");
                CargarContenido(rutaActual);
            }
            else
            {
                await MostrarErrorAsync("No se pudo subir el archivo.");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error inesperado al seleccionar o subir el archivo.");
            Debug.WriteLine($"[ERROR] AñadirArchivoBtn_Clicked: {ex}");
        }
    }

    public async Task<bool> SubirArchivoPdf(string carpeta, string rutaArchivoLocal, string nombreArchivo)
    {
        var url = $"{Config.BackendUrl}/manuales/upload";

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(carpeta), "carpeta");
        content.Add(new StringContent(nombreArchivo), "nombreArchivo");

        using var fileStream = File.OpenRead(rutaArchivoLocal);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        content.Add(fileContent, "file", nombreArchivo);

        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(url, content);

        return response.IsSuccessStatusCode;
    }
}

