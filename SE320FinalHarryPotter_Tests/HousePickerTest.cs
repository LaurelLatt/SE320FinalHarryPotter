using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;
using Xunit;
using System.Collections.Generic;

public class HousePickerTest
{
    private SqliteConnection _connection;
    private SqliteOps _sqliteOps;

    public HousePickerTest()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _sqliteOps = new SqliteOps(_connection);

        string createTableQuery = @"
            CREATE TABLE Houses (
                house_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                founder TEXT,
                mascot TEXT,
                colors TEXT,
                traits TEXT,
                description TEXT
            );
        ";
        _sqliteOps.ModifyQuery(createTableQuery);

        string[] houseNames = { "Ravenclaw", "Slytherin", "Hufflepuff", "Gryffindor" };
        foreach (string name in houseNames)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "@name", name },
                { "@founder", "Founder" },
                { "@mascot", "Mascot" },
                { "@colors", "Blue,Bronze" },
                { "@traits", "Smart,Witty" },
                { "@description", "Some description" }
            };
            _sqliteOps.ModifyQueryWithParams(@"
                INSERT INTO Houses (name, founder, mascot, colors, traits, description)
                VALUES (@name, @founder, @mascot, @colors, @traits, @description)
            ", queryParams);
        }
    }

    [Theory]
    [InlineData("Ravenclaw")]
    [InlineData("Slytherin")]
    [InlineData("Hufflepuff")]
    [InlineData("Gryffindor")]
    public void UserGetHouseCorrectResult(string userInputHouseName)
    {
        HousePicker picker = new HousePicker(new SqlDataAccess(_sqliteOps));
        string result = picker.Evaluate(userInputHouseName);
        Assert.Equal(userInputHouseName, result);
    }

    [Fact]
    public void InvalidHouseReturnsInvalid()
    {
        HousePicker picker = new HousePicker(new SqlDataAccess(_sqliteOps));
        string result = picker.Evaluate("Durmstrang");
        Assert.Equal("Invalid", result);
    }
}
