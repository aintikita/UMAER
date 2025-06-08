using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;


namespace FrontendMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // REGISTRA TU LICENCIA ANTES DE CREAR LA APP  
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhPYVF+WmFZfVtgcF9EYVZQQGYuP1ZhSXxWdkBhUH9acnVUQWNZWUx9XUs=");

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .UseLiveCharts()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
