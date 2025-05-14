namespace SE320FinalHarryPotter;

public class HouseQuery(SqliteOps ops, String name) {
	public String Description {
		get {
			Dictionary<string, string> parms = new();
			parms.Add("@housename", name);
		
			var list = ops.SelectQueryWithParams("select description from Houses where name = @housename limit 1", parms);
			return list[0];
		}
	}
}