using SE320FinalHarryPotter;
using Microsoft.Data.Sqlite;

namespace SE320FinalHarryPotter_Tests;

public class AdminTest
{
    private SqliteOps CreateTestSqliteOps(out SqliteConnection connection)
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
    
        var createCmd = connection.CreateCommand();
        createCmd.CommandText = @"
            CREATE TABLE Houses (
                house_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                founder TEXT,
                mascot TEXT,
                colors TEXT,
                traits TEXT,
                description TEXT
            );";
        createCmd.ExecuteNonQuery();
    
        return new SqliteOps(connection);
    }
    [Fact]
    public void CreateHouseTest()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        Admin admin = new Admin { SqliteOps = sqliteOps };
        
        admin.CreateHouse("Ravenclaw", "Rowena", "Eagle",
            new List<string> { "Blue", "Silver" },
            new List<string> { "Wisdom", "Wit" },
            "Smart and sharp");
        
        List<string> houses = admin.GetHouseList();
        Assert.Single(houses);
        Assert.Equal("1, Ravenclaw, Rowena, Eagle, Blue,Silver, Wisdom,Wit, Smart and sharp", houses[0]);

    }
    
    [Fact]
    public void UpdateHouseDescriptionChangesDescription()
    {
        SqliteOps sqliteOps = CreateTestSqliteOps(out var connection);
        Admin admin = new Admin { SqliteOps = sqliteOps };

        // Insert a house
        admin.CreateHouse("Slytherin", "Salazar", "Snake",
            new List<string> { "Green", "Silver" },
            new List<string> { "Cunning", "Ambition" },
            "Original description");
        

        bool result = admin.UpdateHouseDescription("Slytherin", "Updated description");
        Assert.True(result);

        List<string> updated = sqliteOps.SelectQuery("SELECT description FROM Houses WHERE name = 'Slytherin'");
        Assert.Single(updated);
        Assert.Equal("Updated description", updated[0]);
        
    }
}