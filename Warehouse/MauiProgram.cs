using Microsoft.Extensions.Logging;
using DataBaseContext = Warehouse.Auxiliary.DataBaseContext;
namespace Warehouse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
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
