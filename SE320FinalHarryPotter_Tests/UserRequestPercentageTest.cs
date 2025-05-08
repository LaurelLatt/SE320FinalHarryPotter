using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;
using Xunit;
using System.Collections.Generic;

namespace PotterFinalTest;

public class UserRequestPercentageTest
{
    private SqliteConnection _connection;
    private SqliteOps _sqliteOps;

    public UserRequestPercentageTest()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _sqliteOps = new SqliteOps(_connection);

        // Create Users table
        _sqliteOps.ModifyQuery(@"
            CREATE TABLE Users (
                user_id INTEGER PRIMARY KEY,
                name TEXT,
                house_name TEXT
            );
        ");

        // Insert test users
        var insertUsers = new[]
        {
            "INSERT INTO Users (name, house_name) VALUES ('Harry', 'Gryffindor')",
            "INSERT INTO Users (name, house_name) VALUES ('Hermione', 'Gryffindor')",
            "INSERT INTO Users (name, house_name) VALUES ('Draco', 'Slytherin')",
            "INSERT INTO Users (name, house_name) VALUES ('Luna', 'Ravenclaw')",
        };

        foreach (var query in insertUsers)
        {
            _sqliteOps.ModifyQuery(query);
        }
    }

    [Fact]
    public void TestHousePercentageDistribution()
    {
        var analytics = new HouseAnalytics(_sqliteOps);
        var result = analytics.GetHousePercentages();

        Assert.Equal(50.0, result["Gryffindor"]);  // 2 out of 4
        Assert.Equal(25.0, result["Slytherin"]);    // 1 out of 4
        Assert.Equal(25.0, result["Ravenclaw"]);    // 1 out of 4
    }
}