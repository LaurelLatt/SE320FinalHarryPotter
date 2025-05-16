namespace SE320FinalHarryPotter;

using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class SqliteOps : IDisposable
{
    private SqliteConnection connection;

    // Default constructor for production
    public SqliteOps()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "potterFinaldb");
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
    
    public void EnsureSchema()
    {
        string userTableQuery = @"
        CREATE TABLE IF NOT EXISTS Users (
            user_id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT NOT NULL UNIQUE,
            password TEXT NOT NULL,
            house_name TEXT,
            is_admin INTEGER DEFAULT 0
        );
    ";

        string houseTableQuery = @"
        CREATE TABLE IF NOT EXISTS Houses (
            house_id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL UNIQUE,
            founder TEXT,
            mascot TEXT,
            colors TEXT,
            traits TEXT,
            description TEXT
        );
    ";

        ModifyQuery(userTableQuery);
        ModifyQuery(houseTableQuery);
    }
}