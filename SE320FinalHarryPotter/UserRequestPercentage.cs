namespace SE320FinalHarryPotter;
public class HouseAnalytics
{
    private SqliteOps _sqliteOps;

    public HouseAnalytics(SqliteOps sqliteOps)
    {
        _sqliteOps = sqliteOps;
    }

    public Dictionary<string, double> GetHousePercentages()
    {
        // Step 1: Get total count
        var totalResult = _sqliteOps.SelectQuery("SELECT COUNT(*) FROM Users");
        int totalUsers = int.Parse(totalResult[0]);

        // Step 2: Get count per house
        var houseResults = _sqliteOps.SelectQuery("SELECT house_name, COUNT(*) FROM Users GROUP BY house_name");

        var percentages = new Dictionary<string, double>();
        foreach (var row in houseResults)
        {
            var parts = row.Split(", "); // "Gryffindor, 5"
            string house = parts[0];
            int count = int.Parse(parts[1]);
            double percent = (double)count / totalUsers * 100;
            percentages[house] = percent;
        }

        return percentages;
    }
}
