using SE320FinalHarryPotter;
using Microsoft.Data.Sqlite;

namespace SE320FinalHarryPotter_Tests;

public class AdminTest
{
    private SqliteOps CreateTestSqliteOps(out SqliteConnection connection)
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        SqliteCommand createCmd = connection.CreateCommand();
        createCmd.CommandText = @"
            CREATE TABLE Houses (
                house_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                founder TEXT,
                mascot TEXT,
                colors TEXT,
                traits TEXT,
                description TEXT
            );
            CREATE TABLE Users (
                user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                house_id INTEGER
            );
        ";
        createCmd.ExecuteNonQuery();

        return new SqliteOps(connection);
    }

    [Fact]
    public void CreateHouse_ShouldInsertHouseCorrectlyTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out SqliteConnection? connection);
        SqlDataAccess dataAccess = new SqlDataAccess(sqliteOps); // test instance
        Admin admin = new Admin(dataAccess); // Admin taking IDataAcess

        admin.CreateHouse("Ravenclaw", "Rowena", "Eagle",
            "Blue,Silver", "Wisdom,Wit", "Smart and sharp");

        List<string> houses = admin.GetHouseList();
        Assert.Single(houses);
        Assert.Equal("1, Ravenclaw, Rowena, Eagle, Blue,Silver, Wisdom,Wit, Smart and sharp", houses[0]);
    }

    [Fact]
    public void UpdateHouseDescriptionChangesDescription()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out SqliteConnection? connection);
        SqlDataAccess dataAccess = new SqlDataAccess(sqliteOps); // test instance
        Admin admin = new Admin(dataAccess); // Admin taking IDataAcess

        admin.CreateHouse("Slytherin", "Salazar", "Snake",
            "Green,Silver", "Cunning,Ambition", "Original description");

        bool result = admin.UpdateHouseDescription("Slytherin", "Updated description");

        Assert.True(result);
        List<string> updated = sqliteOps.SelectQuery("SELECT description FROM Houses WHERE name = 'Slytherin'");
        Assert.Single(updated);
        Assert.Equal("Updated description", updated[0]);
    }

    [Fact]
    public void ChangeUserHouse_UpdatesUserHouse_WhenHouseExists()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out SqliteConnection? connection);
        SqlDataAccess dataAccess = new SqlDataAccess(sqliteOps);
        Admin admin = new Admin(dataAccess);

        admin.CreateHouse("Gryffindor", "Godric", "Lion", "Red,Gold", "Bravery", "Brave House");
        admin.CreateHouse("Slytherin", "Salazar", "Snake", "Green,Silver", "Cunning", "Cunning House");

        // Assign Gryffindor (house_id = 1)
        sqliteOps.ModifyQuery("INSERT INTO Users (name, house_id) VALUES ('Harry Potter', 1)");

        bool result = admin.ChangeUserHouse(1, "Slytherin");

        Assert.True(result);

        List<string> updated = sqliteOps.SelectQuery("SELECT house_id FROM Users WHERE user_id = 1");
        string slytherinId = sqliteOps.SelectQuery("SELECT house_id FROM Houses WHERE name = 'Slytherin'")[0];

        Assert.Equal(slytherinId, updated[0]);
    }

}
