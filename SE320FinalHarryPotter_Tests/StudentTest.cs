using SE320FinalHarryPotter;
using Microsoft.Data.Sqlite;
namespace SE320FinalHarryPotter_Tests;

public class StudentTest
{
    private SqliteOps CreateTestSqliteOps(out SqliteConnection connection)
    {
        // Arrange
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        // Create schema
        var setupCommand = connection.CreateCommand();
        setupCommand.CommandText = @"
        CREATE TABLE Houses (
            house_id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT
        );
        CREATE TABLE Users (
            user_id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT,
            house_id INTEGER,
            FOREIGN KEY(house_id) REFERENCES Houses(house_id)
        );

        INSERT INTO Houses (name) VALUES ('Hufflepuff');
        INSERT INTO Users (username, house_id) VALUES ('Cedric', 1);
        INSERT INTO Users (username, house_id) VALUES ('Nymphadora', 1);
    ";
        setupCommand.ExecuteNonQuery();
    
        return new SqliteOps(connection);
    }
    [Fact]
    public void Test_GetStudentCountInHouse_ReturnsCorrectCount()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        Student student = new Student { SqliteOps = sqliteOps };
        
        int count = student.GetStudentCountInHouse("Hufflepuff");

        // Assert
        Assert.Equal(2, count);
    }
}