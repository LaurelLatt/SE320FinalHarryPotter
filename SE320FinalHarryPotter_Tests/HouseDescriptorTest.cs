using System.Runtime.InteropServices;

namespace SE320FinalHarryPotter_Tests;
using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;

public class HouseDescriptorTest {
	private SqliteConnection _connection;
	private SqliteOps _sqliteOps;
	private HouseDescriptor houseDescriptor;

	public HouseDescriptorTest() {
		_connection = new SqliteConnection("Data Source=:memory:");
		_connection.Open();
		_sqliteOps = new SqliteOps(_connection);
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

		string[] houseNames = { "Ravenclaw", "Slytherin", "Hufflepuff", "Gryffindor" };
		foreach (var name in houseNames) {
			var queryParams = new Dictionary<string, string> {
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
			houseDescriptor = new HouseDescriptor(_sqliteOps);
		}
	}

	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescription_Test(string houseName) {
		Assert.Equal("Some description", houseDescriptor.GetHouseDescription(houseName));
	}
}