namespace SE320FinalHarryPotter;

public class HouseDescription(SqliteOps ops) {
	public string GetHouseDescription(string houseName) {
		Dictionary<string, string> parms = new();
		parms.Add("@housename", houseName);
		
		var list = ops.SelectQueryWithParams("select description from Houses where name = @housename limit 1", parms);
		return list[0];
	}
}