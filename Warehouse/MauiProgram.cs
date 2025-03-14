using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Warehouse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Реєстрація IConfiguration
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainPage>();

            var builderDB = 

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        //private static async void SaveSecret()
        //{
        //    await SecureStorage.SetAsync("DefaultConnection", "Host=localhost;Port=5432;Database=.NET;Username=C#;Password=2558");

        //}
    }
}
