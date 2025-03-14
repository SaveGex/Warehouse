using Microsoft.Extensions.Configuration;

public static class AppConfig
{
    public static IConfiguration Configuration { get; }

    static AppConfig()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        Configuration = new ConfigurationBuilder()
            .AddJsonFile(path, optional: false, reloadOnChange: true)
            .Build();
    }


    public static string? GetConnectionString(string name) =>
        Configuration.GetConnectionString(name);
}
