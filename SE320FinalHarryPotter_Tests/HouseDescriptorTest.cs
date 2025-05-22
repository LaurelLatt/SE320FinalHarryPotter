using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;

namespace SE320FinalHarryPotter_Tests;
using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;

public class HouseDescriptorTest {
	private SqliteConnection _connection;
	private SqliteOps _sqliteOps;

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
		foreach (string name in houseNames) {
			Dictionary<string, string> queryParams = new Dictionary<string, string> {
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
		}
	}
	
	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescriptor_TestName(string houseName) {
		HouseDescriptor descriptor = new (_sqliteOps, houseName);
		Assert.Equal(houseName, descriptor.Name);
	}
	
	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescriptor_TestFounder(string houseName) {
		HouseDescriptor descriptor = new (_sqliteOps, houseName);
		Assert.Equal("Founder", descriptor.Founder);
	}
	
	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescriptor_TestMascot(string houseName) {
		HouseDescriptor descriptor = new (_sqliteOps, houseName);
		Assert.Equal("Mascot", descriptor.Mascot);
	}
	
	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescriptor_TestColors(string houseName) {
		HouseDescriptor descriptor = new (_sqliteOps, houseName);
		Assert.Equal(["Blue", "Bronze"], descriptor.Colors);
	}
	
	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescriptor_TestTraits(string houseName) {
		HouseDescriptor descriptor = new (_sqliteOps, houseName);
		Assert.Equal(["Smart", "Witty"], descriptor.Traits);
	}

	[Theory]
	[InlineData("Ravenclaw")]
	[InlineData("Slytherin")]
	[InlineData("Hufflepuff")]
	[InlineData("Gryffindor")]
	public void HouseDescriptor_TestDescription(string houseName) {
		HouseDescriptor descriptor = new (_sqliteOps, houseName);
		Assert.Equal("Some description", descriptor.Description);
	}
}