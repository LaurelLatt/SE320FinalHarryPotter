using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;
using Xunit;

namespace SE320FinalHarryPotter_Tests;


public class SqliteOpsTest
{
        private SqliteOps CreateTestSqlOps(out SqliteConnection connection)
        {
            // Create in-memory SQLite database
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE Users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT
                );

                INSERT INTO Users (username) VALUES ('Laurel');
                INSERT INTO Users (username) VALUES ('Harry');
            ";
            command.ExecuteNonQuery();

            return new SqliteOps(connection);
        }

        [Fact]
        public void SelectQueryReturnsCorrectResult()
        {
            SqliteOps sqliteOps = CreateTestSqlOps(out SqliteConnection? connection);

            List<string> results = sqliteOps.SelectQuery("SELECT username FROM Users");

            Assert.Contains("Laurel", results);
            Assert.Contains("Harry", results);
            
        }

        [Fact]
        public void SelectQueryWithParamsReturnsCorrectResult()
        {
            SqliteOps sqliteOps = CreateTestSqlOps(out SqliteConnection? connection);

            string query = "SELECT username FROM Users WHERE username = @username";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@username", "Laurel" }
            };

            List<string> results = sqliteOps.SelectQueryWithParams(query, parameters);

            Assert.Single(results);
            Assert.Equal("Laurel", results[0]);
            
        }

        [Fact]
        public void ModifyQueryWorksCorrectly()
        {
            SqliteOps sqliteOps = CreateTestSqlOps(out SqliteConnection? connection);

            sqliteOps.ModifyQuery("INSERT INTO Users (username) VALUES ('Ron')");

            List<string> results = sqliteOps.SelectQuery("SELECT username FROM Users");

            Assert.Contains("Ron", results);
            
        }

        [Fact]
        public void ModifyQueryWithParamsWorksCorrectly()
        {
            SqliteOps sqlOps = CreateTestSqlOps(out SqliteConnection connection);

            string query = "INSERT INTO Users (username) VALUES (@username)";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@username", "Hermione" }
            };

            sqlOps.ModifyQueryWithParams(query, parameters);

            List<string> results = sqlOps.SelectQuery("SELECT username FROM Users");

            Assert.Contains("Hermione", results);
            
        }
}