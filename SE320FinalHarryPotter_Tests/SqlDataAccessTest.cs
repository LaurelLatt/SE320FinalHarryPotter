using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;
using Xunit;
using System.Collections.Generic;

namespace SE320FinalHarryPotter_Tests
{
    public class SqlDataAccessTest
    {
        private SqlDataAccess CreateTestAccess(out SqliteConnection connection)
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            SqliteOps sqliteOps = new SqliteOps(connection);
            SqlDataAccess access = new SqlDataAccess(sqliteOps);
            return access;
        }

        [Fact]
        public void CreateAccount_WorksAsExpected()
        {
            SqliteConnection connection;
            SqlDataAccess access = CreateTestAccess(out connection);

            SqliteCommand createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE Users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    password TEXT NOT NULL,
                    house_id INT,
                    class_id INT,
                    is_admin TINYINT DEFAULT 0
                );
            ";
            createTableCmd.ExecuteNonQuery();

            access.CreateAccount("testuser", "pass123");

            List<string> result = access.Login("testuser", "pass123");
            Assert.Single(result);
        }

        [Fact]
        public void Login_WorksAsExpected()
        {
            SqliteConnection connection;
            SqlDataAccess access = CreateTestAccess(out connection);

            SqliteCommand createCmd = connection.CreateCommand();
            createCmd.CommandText = @"
                CREATE TABLE Users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    password TEXT NOT NULL
                );
                INSERT INTO Users (username, password) VALUES ('alice', 'wonderland');
            ";
            createCmd.ExecuteNonQuery();

            List<string> result = access.Login("alice", "wonderland");
            Assert.Single(result);
        }

        [Fact]
        public void SetAdmin_WorksAsExpected()
        {
            SqliteConnection connection;
            SqlDataAccess access = CreateTestAccess(out connection);

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT,
                    password TEXT,
                    is_admin TINYINT DEFAULT 0
                );
                INSERT INTO Users (username, password) VALUES ('bob', 'builder');
            ";
            cmd.ExecuteNonQuery();

            string userId = access.Login("bob", "builder")[0];
            access.SetAdmin(int.Parse(userId));

            SqliteCommand checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT is_admin FROM Users WHERE user_id = " + userId;
            object result = checkCmd.ExecuteScalar();
            Assert.Equal(1L, result);
        }

        [Fact]
        public void GetHouseNames_WorksAsExpected()
        {
            SqliteConnection connection;
            SqlDataAccess access = CreateTestAccess(out connection);

            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Houses (
                    house_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT
                );
                INSERT INTO Houses (name) VALUES ('Gryffindor'), ('Slytherin');
            ";
            cmd.ExecuteNonQuery();

            List<string> result = access.GetHouseNames();
            Assert.Equal(2, result.Count);
            Assert.Contains("Gryffindor", result);
            Assert.Contains("Slytherin", result);
        }
    }
}
