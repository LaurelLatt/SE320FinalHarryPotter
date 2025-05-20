using SE320FinalHarryPotter;

public class HousePicker
{
    private readonly HashSet<string> _validHouses;

    public HousePicker(SqliteOps sqliteOps)
    {
        _validHouses = LoadValidHouses(sqliteOps);
    }

    private HashSet<string> LoadValidHouses(SqliteOps sqliteOps)
    {
        var houses = new HashSet<string>();
        var results = sqliteOps.SelectQuery("SELECT name FROM Houses");

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