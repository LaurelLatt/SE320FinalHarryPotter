namespace SE320FinalHarryPotter;

public class HouseDescriptor(SqliteOps ops, String name) {
	public string Founder => GetSingular("founder");

	public string Mascot => GetSingular("mascot");
	public string[] Colors => GetMultiple("colors");
	public string[] Traits => GetMultiple("traits");

	public string Description => GetSingular("description");

	private string GetSingular(String attribute) {
		Dictionary<string, string> parms = new();
		parms.Add("@name", name);

		// dangerous :O
		var list = ops.SelectQueryWithParams("SELECT " + attribute + " FROM Houses WHERE name = @name LIMIT 1",
			parms);
		return list[0];
	}

	// TODO: Make the query return an array or List to begin with
	private string[] GetMultiple(String attribute) {
		return GetSingular(attribute).Split(',');
	}

}