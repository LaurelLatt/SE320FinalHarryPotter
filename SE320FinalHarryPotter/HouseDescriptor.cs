namespace SE320FinalHarryPotter;

public class HouseDescriptor(SqliteOps ops, String name) {
	public string Founder { get; set; }
	public string Mascot { get; set; }
	public List<string> Colors { get; set; }
	public List<string> Traits { get; set; }
	public String Description {
		get {
			Dictionary<string, string> parms = new();
			parms.Add("@housename", name);
		
			var list = ops.SelectQueryWithParams("SELECT description FROM Houses WHERE name = @housename LIMIT 1", parms);
			return list[0];
		}
	}
}