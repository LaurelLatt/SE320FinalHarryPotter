using System.Runtime.InteropServices;

namespace SE320FinalHarryPotter_Tests;
using Microsoft.Data.Sqlite;
using SE320FinalHarryPotter;

public class HouseDescriptionTest {
	private SqliteConnection _connection;
	private SqliteOps _sqliteOps;
	private HouseDescription _houseDescription;
	
	public HouseDescriptionTest() {
		_connection = new SqliteConnection("Data Source=:memory:");
		_connection.Open();
		_sqliteOps = new SqliteOps(_connection);
		_houseDescription = new HouseDescription(_sqliteOps);
	}

	[Fact]
	public void HouseDescription_Test() {
		Console.WriteLine(_houseDescription.GetHouseDescription("Ravenclaw"));
	}
}