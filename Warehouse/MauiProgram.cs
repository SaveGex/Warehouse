using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Warehouse.DataBase;
using Warehouse;

public static class MauiProgram
{
    public static IServiceProvider? Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddJsonFile("Configurations\\" + "appsettings.json", optional: false, reloadOnChange: true);

        var connectionString = AppConfig.GetConnectionString();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddDbContext<WarehouseContext>(options =>
            options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string is null.")));

        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        Services = app.Services;
        return app;
    }
}
