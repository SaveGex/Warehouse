using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

public static class AppConfig
{
    public static IConfiguration Configuration { get; }

    static AppConfig()
    {
        var path = Path.Join(AppContext.BaseDirectory, "Configurations", "appsettings.json");
        //var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        Configuration = new ConfigurationBuilder()
            .AddJsonFile(path, optional: false, reloadOnChange: true)
            .Build();
    }


    public static string? GetConnectionString()
    {
        using var reader = new StreamReader(Path.Join(AppContext.BaseDirectory, "Configurations", "appsettings.json"));
        var json = reader.ReadToEnd();
        var config = JObject.Parse(json);
        var connectionString = config["ConnectionStrings"]?["DefaultConnection"]?.ToString();
        if(connectionString is null) 
            throw new Exception("Connection string is null. Check your appsettings.json or DI registration.");
        return connectionString;
    }
}
