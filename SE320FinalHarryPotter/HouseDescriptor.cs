namespace SE320FinalHarryPotter;

public class HouseDescriptor(IDataAccess dataAccess, String name) {
	public string Name => name;
	public string Founder => dataAccess.GetHouseFounder(Name);

	public string Mascot => dataAccess.GetHouseMascot(Name);
	public string[] Colors => dataAccess.GetHouseColors(Name);
	public string[] Traits => dataAccess.GetHouseTraits(Name);

	public string Description => dataAccess.GetHouseDescription(Name);

}