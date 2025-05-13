using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;
namespace SE320FinalHarryPotter_Tests;

public class UserTest
{
    private SqliteOps CreateTestSqliteOps(out SqliteConnection connection)
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
    
        var createCmd = connection.CreateCommand();
        createCmd.CommandText = @"
            CREATE TABLE Users (
                user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT NOT NULL,
                password TEXT NOT NULL,
                house_id INT,
                class_id INT,
                is_admin TINYINT DEFAULT 0
            );";
        createCmd.ExecuteNonQuery();
    
        return new SqliteOps(connection);
    }

    [Fact]
    private void CreateAccountSuccessfulTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User{ SqliteOps = sqliteOps };
        
        user.CreateAccount("harry", "potter123");
        
        string query = "SELECT COUNT(*) FROM Users WHERE username = 'harry' AND password = 'potter123'";
        List<string> results = sqliteOps.SelectQuery(query);
        int count = Int32.Parse(results[0]);
        
        Assert.Equal(1, count);
    }

    [Fact]
    private void LoginSuccessfulTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User{ SqliteOps = sqliteOps };
        
        user.CreateAccount("harry", "potter123");
        
        int userID = user.Login("harry", "potter123");
        
        Assert.Equal(1, userID);
    }

    [Fact]
    private void LoginFailedTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User{ SqliteOps = sqliteOps };
        
        user.CreateAccount("harry", "potter123");
        
        int userID = user.Login("harry", "potter");
        Assert.NotEqual(1, userID);
    }

    [Fact]
    private void SetAdminSuccessfulTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User{ SqliteOps = sqliteOps };
        user.CreateAccount("harry", "potter123");
        int userID = user.Login("harry", "potter123");
        user.SetAdmin(userID);

        string query = "SELECT is_admin FROM Users WHERE user_id = @userID";
        Dictionary<string, string> queryParams = new Dictionary<string, string> { { "userID", userID.ToString() } };
        
        List<string> results = sqliteOps.SelectQueryWithParams(query, queryParams);
        int value = Int32.Parse(results[0]);
        
        Assert.Equal(1, value);
    }

    [Fact]
    private void IsUniqueUsernameTrueTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User{ SqliteOps = sqliteOps };
        user.CreateAccount("harry", "potter123");
        user.CreateAccount("ron", "weasley456");
        
        bool result = user.IsUniqueUsername("hermione");
        Assert.True(result);
    }
    
    [Fact]
    private void IsUniqueUsernameFalseTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User{ SqliteOps = sqliteOps };
        user.CreateAccount("harry", "potter123");
        user.CreateAccount("ron", "weasley456");
        
        bool result = user.IsUniqueUsername("harry");
        Assert.False(result);
    }
}