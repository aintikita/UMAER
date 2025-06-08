
using FrontendMAUI.Views.Auth;
using FrontendMAUI.Views.PaginaPrincipal;


namespace FrontendMAUI
{
    public partial class App : Application
    {
        public static bool EsAdmin { get; set; } = false;

        private static DatabaseService _database;
        public static DatabaseService Database =>
            _database ??= new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "enfermeria.db3"));

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new PaginaPrincipalPage());

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                Console.WriteLine($"Excepción no controlada: {ex.Message}\n{ex.StackTrace}");
            };

        }


        public async Task MostrarDebugConstantes()
        {
            var todas = await App.Database.GetConstantesPorPacienteAsync(1); // prueba también con 2
            foreach (var c in todas)
            {
                Console.WriteLine($"Constante ID: {c.id}, PacienteId: {c.pacienteId}, Temp: {c.temperatura}");
            }
        }
    }
}
