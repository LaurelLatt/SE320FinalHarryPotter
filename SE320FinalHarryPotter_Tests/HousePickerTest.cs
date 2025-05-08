namespace SE320FinalHarryPotter_Tests;

using Microsoft.Data.Sqlite;
using Xunit;

public class HousePickerTest
{
    private SqliteConnection _connection;
    private SqliteOps _sqliteOps;

    public HousePickerTest()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _sqliteOps = new SqliteOps(_connection);

        // Create Houses table
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

        // Insert sample data
        string[] houseNames = { "Ravenclaw", "Slytherin", "Hufflepuff", "Gryffindor" };
        foreach (var name in houseNames)
        {
            var queryParams = new Dictionary<string, string>
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
        var picker = new HousePicker(_sqliteOps);
        string result = picker.Evaluate(userInputHouseName);
        Assert.Equal(userInputHouseName, result);
    }

    [Fact]
    public void InvalidHouseReturnsInvalid()
    {
        var picker = new HousePicker(_sqliteOps);
        string result = picker.Evaluate("Durmstrang");
        Assert.Equal("Invalid", result);
    }
}