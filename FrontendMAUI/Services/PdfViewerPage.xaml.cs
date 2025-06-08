namespace FrontendMAUI.Services;

// Página para visualizar un PDF local usando el visor Syncfusion
public partial class PdfViewerPage : ContentPage
{
    private FileStream _stream;

    public PdfViewerPage(string rutaPdf, string nombre)
    {
        InitializeComponent();

        if (File.Exists(rutaPdf))
        {
            try
            {
                _stream = File.OpenRead(rutaPdf);
                pdfViewer.LoadDocument(_stream);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "No se pudo abrir el PDF: " + ex.Message, "OK");
            }
        }
        else
        {
            DisplayAlert("Error", "Archivo no encontrado.", "OK");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _stream?.Dispose(); // cierra el stream al salir
    }
}




