using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Warehouse.DataBase;
using Warehouse;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var connectionString = getConnectionString();

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

    // Метод отримання рядка з'єднання як у твоєму прикладі
    public static string getConnectionString()
    {
        string jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "appsettings.json");
        string connectionString = (string)JObject.Parse(jsonString)["ConnectionStrings"]["DefaultConnection"];

        if (connectionString == null)
            throw new Exception("Connection string is null. Check your appsettings.json or DI registration.");

        return connectionString;
    }
}
