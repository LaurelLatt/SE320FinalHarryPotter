namespace SE320FinalHarryPotter;

public class Admin
{
    public SqliteOps SqliteOps = new SqliteOps();
    
    public void CreateHouse(string name, string founder, string mascot, List<string> colors, List<string> traits, string description)
    {

        string query = @"INSERT INTO Houses(name, founder, mascot, colors, traits, description)
                        VALUES (@name, @founder, @mascot, @colors, @traits, @description)
                        ";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@name", name },
            { "@founder", founder },
            { "@mascot", mascot },
            { "@colors", string.Join(",", colors) },
            { "@traits", string.Join(",", traits) },
            { "@description", description }
        };
        SqliteOps.ModifyQueryWithParams(query, queryParams);
    }
    
    public List<string> GetHouseList()
    {
        string query = @"SELECT * FROM Houses";
        List<string> houses = SqliteOps.SelectQuery(query);
        return houses;
    }
    
    public string GetHouseByName(string name)
    {
        string query = @"SELECT house_id FROM Houses WHERE name = @name";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@name", name }
        };
        List<string> houseID= SqliteOps.SelectQueryWithParams(query, queryParams);
        return houseID[0];
    }
    
    public bool UpdateHouseDescription(string houseName, string newDescription)
    {
        string houseID = GetHouseByName(houseName);
        if (houseID != " ")
        {
            string query = "UPDATE Houses SET description = @newDescription WHERE house_id = @houseId";
            Dictionary<string, string> queryParams = new Dictionary<string, string>()
            {
                { "@newDescription", newDescription },
                { "@houseId", houseID }
            };
            SqliteOps.ModifyQueryWithParams(query, queryParams);
            return true;
        }

        return false; //house not found
    }
}