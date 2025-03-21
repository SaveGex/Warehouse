using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Warehouse.DataBase
{
    internal static class WarehouseStaticContext
    {
        private static readonly string? connectionString;

        public static int countOfIndefinedElementsWhenWasLastUpdated { get; set; } = 0;


        static WarehouseStaticContext()
        {
            connectionString = MauiProgram.getConnectionString();

            if (connectionString == null)
                throw new Exception("Connection string is null. Check your appsettings.json or DI registration.");
        }


        // Метод для виконання запитів SELECT
        public static async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string query)
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }

                            results.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR]: {ex.Message}");
            }

            return results;
        }


        public static List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }

                            results.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR]: {ex.Message}");
            }

            return results;
        }


        // Метод для виконання INSERT, UPDATE, DELETE із параметрами
        public static async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Додаємо параметри
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logs.txt");
                await File.WriteAllTextAsync(path, $"[DB ERROR]: {ex.Message}");

                return -1;
            }
        }


    }
}
