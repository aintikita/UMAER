using FrontendMAUI.Models;
using FrontendMAUI.Services;
using FrontendMAUI.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.ComponentModel;

namespace FrontendMAUI.Views.Enfermeria;

public partial class GraficaConstantesPage : ContentPage
{
    private readonly Paciente _paciente;
    private List<Constante> _registros = new();
    private readonly GraficaConstantesViewModel _viewModel;

    public GraficaConstantesPage(Paciente paciente)
    {
        InitializeComponent();
        _paciente = paciente;
        _viewModel = new GraficaConstantesViewModel();
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        BindingContext = _viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    // Método auxiliar para mostrar mensajes de error amigables
    private async Task MostrarErrorAsync(string mensaje)
    {
        await DisplayAlert("Error", mensaje, "OK");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (_paciente.Id == null)
            {
                await MostrarErrorAsync("No se pudo identificar el paciente.");
                return;
            }
            _registros = await ApiService.GetConstantesPorPacienteAsync(_paciente.Id.Value);
            ActualizarGrafica();
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("No se pudieron cargar los datos de constantes. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] Cargar constantes: {ex}");
        }
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        try
        {
            if (e.PropertyName is nameof(_viewModel.MostrarTemperatura)
                or nameof(_viewModel.MostrarFC)
                or nameof(_viewModel.MostrarFR)
                or nameof(_viewModel.MostrarTsis)
                or nameof(_viewModel.MostrarTdia)
                or nameof(_viewModel.MostrarSat)
                or nameof(_viewModel.MostrarGlu))
            {
                ActualizarGrafica();
            }
        }
        catch (Exception ex)
        {
            // No se muestra al usuario, pero se registra para depuración
            Console.WriteLine($"[ERROR] Actualizar gráfica: {ex}");
        }
    }

    private void ActualizarGrafica()
    {
        try
        {
            if (_registros.Count == 0) return;

            var labels = _registros.OrderBy(x => x.fechaHora)
                                   .Select(x => x.fechaHora.ToString("dd/MM HH:mm"))
                                   .ToArray();

            var series = new List<ISeries>();

            if (_viewModel.MostrarTemperatura)
                series.Add(CrearSerie("Temp °C", _registros.Select(r => (double)(r.temperatura ?? 0)).ToArray(), SKColors.Orange));

            if (_viewModel.MostrarFC)
                series.Add(CrearSerie("FC", _registros.Select(r => (double)(r.frecuenciaCardiaca ?? 0)).ToArray(), SKColors.Red));

            if (_viewModel.MostrarFR)
                series.Add(CrearSerie("FR", _registros.Select(r => (double)(r.frecuenciaRespiratoria ?? 0)).ToArray(), SKColors.Green));

            if (_viewModel.MostrarTsis)
                series.Add(CrearSerie("T. Sistolica", _registros.Select(r => ParsePresion(r.presionArterial, true)).ToArray(), SKColors.Blue));

            if (_viewModel.MostrarTdia)
                series.Add(CrearSerie("T. Diastolica", _registros.Select(r => ParsePresion(r.presionArterial, false)).ToArray(), SKColors.Purple));

            if (_viewModel.MostrarSat)
                series.Add(CrearSerie("Sat O2", _registros.Select(r => (double)(r.saturacionOxigeno ?? 0)).ToArray(), SKColors.Teal));

            _viewModel.ChartSeries = series.ToArray();
            _viewModel.XAxes = new Axis[] { new Axis { Labels = labels, LabelsRotation = 45, TextSize = 12 } };
            _viewModel.YAxes = new Axis[] { new Axis { TextSize = 12 } };
        }
        catch (Exception ex)
        {
            // No se muestra al usuario, pero se registra para depuración
            Console.WriteLine($"[ERROR] ActualizarGrafica: {ex}");
        }
    }

    private double ParsePresion(string? presion, bool sistolica)
    {
        if (string.IsNullOrWhiteSpace(presion)) return 0;
        var partes = presion.Split('/');
        if (partes.Length != 2) return 0;

        return double.TryParse(sistolica ? partes[0] : partes[1], out var resultado) ? resultado : 0;
    }

    private ISeries CrearSerie(string nombre, double[] valores, SKColor color)
    {
        return new LineSeries<double>
        {
            Name = nombre,
            Values = valores,
            Stroke = new SolidColorPaint(color, 3),
            GeometrySize = 6,
            Fill = null
        };
    }

    private async void OnExportarPdfClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        if (boton != null)
        {
            var colorOriginal = boton.BackgroundColor; // Guarda el color original

            boton.BackgroundColor = Color.FromArgb("#7cc191"); // Cambia al color al hacer click
            await Task.Delay(300); // Retardo de 200ms para ver claramente el cambio visual
            boton.BackgroundColor = colorOriginal; // Vuelve al color original

        }
        try
        {
            if (_paciente.Id == null)
            {
                await MostrarErrorAsync("No se pudo identificar el paciente.");
                return;
            }

            var constantes = await ApiService.GetConstantesPorPacienteAsync(_paciente.Id.Value);
            var medicaciones = await ApiService.GetMedicacionPorPacienteAsync(_paciente.Id.Value);
            var diuresis = await ApiService.GetDiuresisPorPacienteAsync(_paciente.Id.Value);
            var balances = await ApiService.GetBalancePorPacienteAsync(_paciente.Id.Value);
            var alergias = await ApiService.GetAlergiasPorPacienteAsync(_paciente.Id.Value);
            var cuidados = await ApiService.GetCuidadosPorPacienteAsync(_paciente.Id.Value);

            var tiposSeleccionados = new List<string>();
            if (_viewModel.MostrarTemperatura) tiposSeleccionados.Add("Temperatura");
            if (_viewModel.MostrarFC) tiposSeleccionados.Add("Frecuencia Cardíaca");
            if (_viewModel.MostrarFR) tiposSeleccionados.Add("Frecuencia Respiratoria");
            if (_viewModel.MostrarTsis) tiposSeleccionados.Add("Tensión Sistólica");
            if (_viewModel.MostrarTdia) tiposSeleccionados.Add("Tensión Diastólica");
            if (_viewModel.MostrarSat) tiposSeleccionados.Add("Saturación");

            var graficoStream = GraficaImageGenerator.GenerarGrafica(constantes, tiposSeleccionados.ToArray());

            string nombreFirmante = "";
            var pdfService = new PdfGeneratorService();
            byte[] pdfBytes;
            try
            {
                pdfBytes = await pdfService.GenerarPdfAsync(_paciente, constantes, medicaciones, diuresis, balances, alergias, cuidados, nombreFirmante, graficoStream);
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("No se pudo generar el PDF. Inténtalo de nuevo.");
                Console.WriteLine($"[ERROR] Generar PDF: {ex}");
                return;
            }

            try
            {
                var filename = $"Reporte_{_paciente.Nombre}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                var ruta = Path.Combine(FileSystem.CacheDirectory, filename);
                File.WriteAllBytes(ruta, pdfBytes);
                await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(ruta) });
            }
            catch (Exception ex)
            {
                await MostrarErrorAsync("No se pudo guardar o abrir el PDF generado.");
                Console.WriteLine($"[ERROR] Guardar/abrir PDF: {ex}");
            }
        }
        catch (Exception ex)
        {
            await MostrarErrorAsync("Ocurrió un error al exportar el PDF. Inténtalo de nuevo.");
            Console.WriteLine($"[ERROR] OnExportarPdfClicked: {ex}");
        }
    }
}
