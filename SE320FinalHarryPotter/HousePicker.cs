using SE320FinalHarryPotter;

public class HousePicker
{
    private HashSet<string> validHouses;

    public HousePicker(SqliteOps sqliteOps)
    {
        validHouses = new HashSet<string>();
        string query = "SELECT name FROM Houses";
        var houseNames = sqliteOps.SelectQuery(query);

        foreach (var name in houseNames)
        {
            validHouses.Add(name.Trim()); // Defensive: trim whitespace
        }
    }

    public string Evaluate(string userInput)
    {
        return validHouses.Contains(userInput.Trim()) ? userInput : "Invalid";
    }
}