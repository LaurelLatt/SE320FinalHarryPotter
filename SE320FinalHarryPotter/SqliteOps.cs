namespace SE320FinalHarryPotter;

using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class SqliteOps : IDisposable
{
    private SqliteConnection connection;

    // Default constructor for production
    public SqliteOps()
    {
        string dbPath = Path.Combine(AppContext.BaseDirectory, "potterFinaldb");
        Console.WriteLine($"dbPath: {dbPath}");
        connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
    }

    // Test constructor – accepts any connection (like an in-memory or test file DB)
    public SqliteOps(SqliteConnection customConnection)
    {
        connection = customConnection;
        connection.Open();
    }

    public void Dispose()
    {
        connection.Close();
        connection.Dispose();
    }

    public List<string> SelectQuery(string query)
    {
        List<string> result = new List<string>();
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int fieldCount = reader.FieldCount;
                    string row = "";
                    for (int i = 0; i < fieldCount; i++)
                    {
                        row += reader[i].ToString();
                        if (i < fieldCount - 1)
                            row += ", ";
                    }
                    result.Add(row);
                }
            }
        }
        
        return result;
    }

    public List<string> SelectQueryWithParams(string query, Dictionary<string, string> queryParams)
    {
        List<string> result = new List<string>();
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var param in queryParams)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int fieldCount = reader.FieldCount;
                    string row = "";
                    for (int i = 0; i < fieldCount; i++)
                    {
                        row += reader[i].ToString();
                        if (i < fieldCount - 1)
                            row += ", ";
                    }
                    result.Add(row);
                }
            }
        }
        
        return result;
    }

    public void ModifyQuery(string query)
    {
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
        
    }

    public void ModifyQueryWithParams(string query, Dictionary<string, string> queryParams)
    {
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var param in queryParams)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
            command.ExecuteNonQuery();
        }
        
    }
}