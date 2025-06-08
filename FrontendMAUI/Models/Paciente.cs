namespace FrontendMAUI
{
    public class Paciente
    {
        public int? Id { get; set; } // Id del paciente
        public string Nombre { get; set; } // Nombre completo
        public string Habitacion { get; set; } // Nº de habitación
        public string Unidad { get; set; } // Unidad hospitalaria
        public int Edad { get; set; } // Edad
        public float Peso { get; set; } // Peso en kg
        public float Altura { get; set; } // Altura en cm
    }

    public class Constante
    {
        public int? id { get; set; } // Id del registro
        public int pacienteId { get; set; } // Id del paciente
        public DateTime fechaHora { get; set; } // Fecha y hora del registro
        public float? temperatura { get; set; } // Temperatura corporal
        public int? frecuenciaCardiaca { get; set; } // Frecuencia cardiaca
        public int? frecuenciaRespiratoria { get; set; } // Frecuencia respiratoria
        public int? saturacionOxigeno { get; set; } // Saturación de oxígeno
        public string presionArterial { get; set; } // Presión arterial
    }

    public class Medicacion
    {
        public int? id { get; set; } // Id del registro
        public int pacienteId { get; set; } // Id del paciente
        public DateTime fechaHora { get; set; } // Fecha y hora de administración
        public string medicamento { get; set; } // Nombre del medicamento
        public string dosis { get; set; } // Dosis administrada
        public string frecuencia { get; set; } // Frecuencia de administración
        public string observaciones { get; set; } // Observaciones
    }

    public class Diuresis
    {
        public int? id { get; set; } // Id del registro
        public int pacienteId { get; set; } // Id del paciente
        public DateTime fechaHora { get; set; } // Fecha y hora del registro
        public float cantidad { get; set; } // Cantidad de diuresis
        public string observaciones { get; set; } // Observaciones
    }

    public class BalanceHidrico
    {
        public int? id { get; set; } // Id del registro
        public int pacienteId { get; set; } // Id del paciente
        public DateTime fechaHora { get; set; } // Fecha y hora del registro
        public float ingreso { get; set; } // Ingreso de líquidos
        public float egreso { get; set; } // Egreso de líquidos
        public string observaciones { get; set; } // Observaciones
    }

    public class Alergia
    {
        public int? id { get; set; } // Id del registro
        public int pacienteId { get; set; } // Id del paciente
        public string alergia { get; set; } // Nombre de la alergia
        public string descripcion { get; set; } // Descripción de la alergia
    }

    public class CuidadoEnfermeria
    {
        public int? id { get; set; } // Id del registro
        public int pacienteId { get; set; } // Id del paciente
        public DateTime fechaHora { get; set; } // Fecha y hora del cuidado
        public string descripcion { get; set; } // Descripción del cuidado
    }

    public class PdfReporte
    {
        public string id { get; set; } // Id del reporte PDF
        public string descripcion { get; set; } // Descripción del reporte
    }

    public class ReportePdf
    {
        public string Archivo { get; set; } // Archivo PDF en base64 o ruta
        public string Mensaje { get; set; } // Mensaje de estado o error
    }
}

