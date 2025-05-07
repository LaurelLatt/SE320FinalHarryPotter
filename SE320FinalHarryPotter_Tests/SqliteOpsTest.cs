using Microsoft.Data.Sqlite;
using PotterFinal;
using Xunit;

namespace PotterFinalTest;


public class SqliteOpsTest
{
        private SqliteOps CreateTestSqlOps(out SqliteConnection connection)
        {
            // Create in-memory SQLite database
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var command = connection.CreateCommand();
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
            var sqliteOps = CreateTestSqlOps(out var connection);

            var results = sqliteOps.SelectQuery("SELECT username FROM Users");

            Assert.Contains("Laurel", results);
            Assert.Contains("Harry", results);
            
        }

        [Fact]
        public void SelectQueryWithParamsReturnsCorrectResult()
        {
            var sqliteOps = CreateTestSqlOps(out var connection);

            var query = "SELECT username FROM Users WHERE username = @username";
            var parameters = new Dictionary<string, string>
            {
                { "@username", "Laurel" }
            };

            var results = sqliteOps.SelectQueryWithParams(query, parameters);

            Assert.Single(results);
            Assert.Equal("Laurel", results[0]);
            
        }

        [Fact]
        public void ModifyQueryWorksCorrectly()
        {
            var sqliteOps = CreateTestSqlOps(out var connection);

            sqliteOps.ModifyQuery("INSERT INTO Users (username) VALUES ('Ron')");

            var results = sqliteOps.SelectQuery("SELECT username FROM Users");

            Assert.Contains("Ron", results);
            
        }

        [Fact]
        public void ModifyQueryWithParamsWorksCorrectly()
        {
            var sqlOps = CreateTestSqlOps(out var connection);

            string query = "INSERT INTO Users (username) VALUES (@username)";
            var parameters = new Dictionary<string, string>
            {
                { "@username", "Hermione" }
            };

            sqlOps.ModifyQueryWithParams(query, parameters);

            var results = sqlOps.SelectQuery("SELECT username FROM Users");

            Assert.Contains("Hermione", results);
            
        }
}