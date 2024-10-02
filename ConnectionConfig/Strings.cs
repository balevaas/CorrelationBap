using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ConnectionConfig
{
    public static class Strings
    {
        public const string
            Sqlite = "Sqlite",
            SqlExpress = "SqlExpress";

        public static string GetConnectionStrings(string key)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream("ConnectionConfig.appsettings.json");
            var builder = new ConfigurationBuilder();

            builder.AddJsonStream(resourceStream!);
            var config = builder.Build();
            var connectionString = config.GetConnectionString(key);
            return connectionString ?? throw new ArgumentException($"Такой строки подключения не существует: {key}", key);
        }

    }
}
