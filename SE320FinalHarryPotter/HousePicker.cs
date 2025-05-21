using SE320FinalHarryPotter;

public class HousePicker
{
    private readonly HashSet<string> _validHouses;

    public HousePicker(IDataAccess dataAccess)
    {
        _validHouses = LoadValidHouses(dataAccess);
    }

    private HashSet<string> LoadValidHouses(IDataAccess dataAccess)
    {
        HashSet<string> houses = new HashSet<string>();
        List<string> results = dataAccess.GetHouseNames();

        foreach (var name in results)
        {
            if (!string.IsNullOrWhiteSpace(name))
                houses.Add(name.Trim());
        }

        return houses;
    }

    public string Evaluate(string? userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
            return "Invalid";

        string trimmedInput = userInput.Trim();
        return _validHouses.Contains(trimmedInput) ? trimmedInput : "Invalid";
    }
}