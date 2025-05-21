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
            );
            CREATE TABLE Houses (
                house_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT
            );
            INSERT INTO Houses (name) VALUES ('Hufflepuff');
            INSERT INTO Users (username, password, house_id) VALUES ('Cedric', 'Diggory123', 1);
            INSERT INTO Users (username, password, house_id) VALUES ('Nymphadora', 'Tonks456', 1);
            ";
        createCmd.ExecuteNonQuery();
    
        return new SqliteOps(connection);
    }

    [Fact]
    private void CreateAccountSuccessfulTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User(new SqlDataAccess(sqliteOps));
        
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
        User user = new User(new SqlDataAccess(sqliteOps));
        
        user.CreateAccount("harry", "potter123");
        
        int userID = user.Login("harry", "potter123");
        
        Assert.Equal(3, userID);
    }

    [Fact]
    private void LoginFailedTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User(new SqlDataAccess(sqliteOps));
        
        user.CreateAccount("harry", "potter123");
        
        int userID = user.Login("harry", "potter");
        Assert.NotEqual(3, userID);
    }

    [Fact]
    private void SetAdminSuccessfulTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User(new SqlDataAccess(sqliteOps));
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
        User user = new User(new SqlDataAccess(sqliteOps));
        user.CreateAccount("harry", "potter123");
        user.CreateAccount("ron", "weasley456");
        
        bool result = user.IsUniqueUsername("hermione");
        Assert.True(result);
    }
    
    [Fact]
    private void IsUniqueUsernameFalseTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User(new SqlDataAccess(sqliteOps));
        user.CreateAccount("harry", "potter123");
        user.CreateAccount("ron", "weasley456");
        
        bool result = user.IsUniqueUsername("harry");
        Assert.False(result);
    }
    
    [Fact]
    public void GetStudentCountInHouseSuccessfulTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        User user = new User(new SqlDataAccess(sqliteOps));
        
        int count = user.GetStudentCountInHouse("Hufflepuff");
        
        Assert.Equal(2, count);
    }
    
    [Fact]
    public void GetHouseMembershipPercentages_ReturnsCorrectValues()
    {
        // Setup in-memory DB with Users table including house_name
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var sqliteOps = new SqliteOps(connection);

        var setupCmd = connection.CreateCommand();
        setupCmd.CommandText = @"
        CREATE TABLE Users (
            user_id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT,
            password TEXT,
            house_name TEXT
        );
    ";
        setupCmd.ExecuteNonQuery();

        // Insert test data
        var insertUsers = new[]
        {
            "INSERT INTO Users (username, password, house_name) VALUES ('Harry', 'pw', 'Gryffindor')",
            "INSERT INTO Users (username, password, house_name) VALUES ('Hermione', 'pw', 'Gryffindor')",
            "INSERT INTO Users (username, password, house_name) VALUES ('Draco', 'pw', 'Slytherin')",
            "INSERT INTO Users (username, password, house_name) VALUES ('Luna', 'pw', 'Ravenclaw')"
        };

        foreach (var query in insertUsers)
        {
            sqliteOps.ModifyQuery(query);
        }

        // Use the public SqliteOps property in User (matching your current implementation)
        var user = new User(new SqlDataAccess(sqliteOps));
        
        // let's not write drunk code next time, okay?
        //user.SqliteOps = sqliteOps;

        var result = user.GetHouseMembershipPercentages();

        Assert.Equal(50.0, result["Gryffindor"]);
        Assert.Equal(25.0, result["Slytherin"]);
        Assert.Equal(25.0, result["Ravenclaw"]);
    }
    
}