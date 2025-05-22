using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;
using Xunit;
using System.Collections.Generic;

namespace SE320FinalHarryPotter_Tests
{
    public class IDataAccessTest
    {
        private IDataAccess CreateTestAccess(out SqliteConnection connection)
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            SqliteOps sqliteOps = new SqliteOps(connection);
            IDataAccess access = new SqlDataAccess(sqliteOps);

            return access;
        }

        [Fact]
        public void CreateAccount_And_Login_WorksThroughInterface()
        {
            SqliteConnection connection;
            IDataAccess access = CreateTestAccess(out connection);

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    password TEXT NOT NULL
                );
            ";
            cmd.ExecuteNonQuery();

            access.CreateAccount("hermione", "leviosa");
            List<string> result = access.Login("hermione", "leviosa");

            Assert.Single(result);
        }

        [Fact]
        public void IsUniqueUsername_WorksThroughInterface()
        {
            SqliteConnection connection;
            IDataAccess access = CreateTestAccess(out connection);

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    password TEXT NOT NULL
                );
                INSERT INTO Users (username, password) VALUES ('ron', 'weasley');
            ";
            cmd.ExecuteNonQuery();

            bool isUnique = access.IsUniqueUsername("harry");
            bool isNotUnique = access.IsUniqueUsername("ron");

            Assert.True(isUnique);
            Assert.False(isNotUnique);
        }

        [Fact]
        public void GetHouseNames_WorksThroughInterface()
        {
            SqliteConnection connection;
            IDataAccess access = CreateTestAccess(out connection);

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Houses (
                    house_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT
                );
                INSERT INTO Houses (name) VALUES ('Hufflepuff'), ('Ravenclaw');
            ";
            cmd.ExecuteNonQuery();

            List<string> result = access.GetHouseNames();
            Assert.Equal(2, result.Count);
            Assert.Contains("Hufflepuff", result);
            Assert.Contains("Ravenclaw", result);
        }
    }
}
