using System.Collections.Generic;
using PotterFinal;

public class HousePicker
{
    private HashSet<string> validHouses;

    public HousePicker(SqliteOps sqliteOps)
    {
        validHouses = new HashSet<string>();
        string query = "SELECT name FROM Houses";
        var houseNames = sqliteOps.SelectQuery(query);
        
        foreach (var row in houseNames)
        {
            validHouses.Add(row.Split(',')[0]); // In case your rows have more than just the name
        }
    }

    public string Evaluate(string userInput)
    {
        return validHouses.Contains(userInput) ? userInput : "Invalid";
    }
}