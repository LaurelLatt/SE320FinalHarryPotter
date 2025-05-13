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
    
    // Change a user's house based on their user_id
    public bool ChangeUserHouse(int userId, string newHouseName)
    {
        // Validate the house exists
        var houseCheck = SqliteOps.SelectQueryWithParams(
            "SELECT house_id FROM Houses WHERE name = @name",
            new Dictionary<string, string> { { "@name", newHouseName } }
        );

        if (houseCheck.Count == 0)
        {
            return false; // House not found
        }

        // Update the user's house
        SqliteOps.ModifyQueryWithParams(
            "UPDATE Users SET house_name = @newHouse WHERE user_id = @userId",
            new Dictionary<string, string>
            {
                { "@newHouse", newHouseName },
                { "@userId", userId.ToString() }
            }
        );

        return true;
    }

}