using FrontendMAUI;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontendMAUI
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db; // Conexión a la base de datos SQLite

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Paciente>().Wait(); // Crea la tabla Paciente si no existe
            _db.CreateTableAsync<Constante>().Wait(); // Crea la tabla Constante si no existe
        }

        public Task<List<Paciente>> GetPacientesAsync() =>
            _db.Table<Paciente>().ToListAsync(); // Obtiene todos los pacientes

        public Task<int> SavePacienteAsync(Paciente p) =>
            p.Id != 0 ? _db.UpdateAsync(p) : _db.InsertAsync(p); // Inserta o actualiza un paciente

        public Task<List<Constante>> GetConstantesPorPacienteAsync(int pacienteId) =>
            _db.Table<Constante>()
               .Where(c => c.pacienteId == pacienteId)
               .OrderByDescending(c => c.fechaHora)
               .ToListAsync(); // Constantes de un paciente, ordenadas por fecha

        public Task<int> SaveConstantesAsync(Constante c) =>
            _db.InsertAsync(c); // Inserta una constante

        public async Task EliminarPacienteAsync(Paciente paciente)
        {
            // Elimina primero las constantes del paciente
            await _db.Table<Constante>().Where(c => c.pacienteId == paciente.Id).DeleteAsync();

            // Luego elimina el paciente
            await _db.DeleteAsync(paciente);
        }

        public Task EliminarConstantesPorPacienteAsync(int pacienteId)
        {
            return _db.Table<Constante>().Where(c => c.pacienteId == pacienteId).DeleteAsync(); // Elimina todas las constantes de un paciente
        }
    }
}

