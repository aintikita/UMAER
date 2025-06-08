using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FrontendMAUI.ViewModels;

// ViewModel para gestionar la visualización de las constantes en la gráfica
public class GraficaConstantesViewModel : INotifyPropertyChanged
{
    // Evento para notificar cambios en las propiedades (para el binding)
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // Propiedades para mostrar/ocultar cada tipo de constante en la gráfica
    private bool _mostrarTemperatura, _mostrarFC, _mostrarFR, _mostrarTsis, _mostrarTdia, _mostrarSat, _mostrarGlu;

    public bool MostrarTemperatura { get => _mostrarTemperatura; set { _mostrarTemperatura = value; OnPropertyChanged(); } } // Temperatura
    public bool MostrarFC { get => _mostrarFC; set { _mostrarFC = value; OnPropertyChanged(); } } // Frecuencia cardiaca
    public bool MostrarFR { get => _mostrarFR; set { _mostrarFR = value; OnPropertyChanged(); } } // Frecuencia respiratoria
    public bool MostrarTsis { get => _mostrarTsis; set { _mostrarTsis = value; OnPropertyChanged(); } } // Presión sistólica
    public bool MostrarTdia { get => _mostrarTdia; set { _mostrarTdia = value; OnPropertyChanged(); } } // Presión diastólica
    public bool MostrarSat { get => _mostrarSat; set { _mostrarSat = value; OnPropertyChanged(); } } // Saturación de oxígeno
    public bool MostrarGlu { get => _mostrarGlu; set { _mostrarGlu = value; OnPropertyChanged(); } } // Glucosa

    // Series de datos que se muestran en la gráfica
    private ISeries[] _chartSeries = Array.Empty<ISeries>();
    public ISeries[] ChartSeries { get => _chartSeries; set { _chartSeries = value; OnPropertyChanged(); } }

    // Ejes X de la gráfica
    private Axis[] _xAxes = Array.Empty<Axis>();
    public Axis[] XAxes { get => _xAxes; set { _xAxes = value; OnPropertyChanged(); } }

    // Ejes Y de la gráfica
    private Axis[] _yAxes = Array.Empty<Axis>();
    public Axis[] YAxes { get => _yAxes; set { _yAxes = value; OnPropertyChanged(); } }
}



