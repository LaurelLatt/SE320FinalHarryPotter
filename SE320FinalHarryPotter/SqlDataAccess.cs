namespace SE320FinalHarryPotter;

public class SqlDataAccess : IDataAccess {
    // To be able to reuse code, pull out database methods so that
    // you can create a method in a new project that manipulates a different database

    private SqliteOps sqliteOps = new SqliteOps();
    
    //  Constructor 1 - Default for production
    public SqlDataAccess()
    {
        sqliteOps = new SqliteOps();
    }

    // Constructor 2 - For testing (injection)
    public SqlDataAccess(SqliteOps customOps)
    {
        sqliteOps = customOps;
    }

    public List<string> GetHouseNames()
    {
        return sqliteOps.SelectQuery("SELECT name FROM Houses");
    }

    public string GetHouseDescription(int houseId)
    {
        // do the sql stuff
        return string.Empty;
    }

    public void CreateAccount(string username, string password)
    {
        string query = "INSERT INTO Users(username, password) VALUES (@username, @password)";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@username", username },
            { "@password", password },
        };
        sqliteOps.ModifyQueryWithParams(query, queryParams);
    }

    public List<string> Login(string username, string password)
    {
        string query = "SELECT user_id FROM Users WHERE username=@username AND password=@password";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@username", username },
            { "@password", password }
        };
        return sqliteOps.SelectQueryWithParams(query, parameters);
    }

    public void SetAdmin(int userID)
    {
        string query = "UPDATE Users SET is_admin=1 WHERE user_id=@userID";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@userID", userID.ToString() }
        }; 
        sqliteOps.ModifyQueryWithParams(query, parameters);
    }

    public bool IsUniqueUsername(string username)
    {
        string query = "SELECT COUNT(*) FROM Users WHERE username=@username";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@username", username }
        };
        List<string> output = sqliteOps.SelectQueryWithParams(query, parameters);
        return output[0] == "0";
    }

    public int GetStudentCountInHouse(string houseName)
    {
        string query = @"SELECT COUNT(*)
                FROM Users as U 
                INNER JOIN Houses as H 
                    ON U.house_id = H.house_id
                WHERE H.name = @houseName;";

        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@houseName", houseName }
        };
        List<string> count = sqliteOps.SelectQueryWithParams(query, queryParams);
        
        return Int32.Parse(count[0]);
    }

    public int GetTotalStudentCount()
    {
        List<string> totalUsersResult = sqliteOps.SelectQuery("SELECT COUNT(*) FROM Users");
        int totalUsers = int.Parse(totalUsersResult[0]);
        return totalUsers;
    }

    public List<string> GetStudentCountInHouseList()
    {
        return sqliteOps.SelectQuery("SELECT h.name, COUNT(u.user_id) FROM Users AS u INNER JOIN Houses AS h ON u.house_id = h.house_id GROUP BY h.name");
    }

    public void CreateHouse(string name, string founder, string mascot, string colors, string traits, string description)
    {
        string query = @"INSERT INTO Houses(name, founder, mascot, colors, traits, description)
                        VALUES (@name, @founder, @mascot, @colors, @traits, @description)
                        ";
        Dictionary<string, string> queryParams = new()
        {
            { "@name", name },
            { "@founder", founder },
            { "@mascot", mascot },
            { "@colors", colors },
            { "@traits", traits },
            { "@description", description }
        };
        sqliteOps.ModifyQueryWithParams(query, queryParams);
    }

    public List<string> GetHouseList()
    {
        string query = @"SELECT * FROM Houses";
        return sqliteOps.SelectQuery(query);
    }

    public string GetHouseIdByName(string houseName)
    {
        string query = @"SELECT house_id FROM Houses WHERE name = @name";
        Dictionary<string, string> queryParams = new()
        {
            { "@name", houseName }
        };
        List<string> houseID= sqliteOps.SelectQueryWithParams(query, queryParams);
        return houseID.Count > 0 ? houseID[0] : "";
    }

    public void UpdateHouseDescription(string houseId, string newDescription)
    {
        string query = "UPDATE Houses SET description = @newDescription WHERE house_id = @houseId";
        Dictionary<string, string> queryParams = new()
        {
            { "@newDescription", newDescription },
            { "@houseId", houseId }
        };
        sqliteOps.ModifyQueryWithParams(query, queryParams);
    }

    public void UpdateUserHouse(int userId, string newHouseId)
    {
        sqliteOps.ModifyQueryWithParams(
            "UPDATE Users SET house_id = @newHouse WHERE user_id = @userId",
            new Dictionary<string, string>
            {
                { "@newHouse", newHouseId },
                { "@userId", userId.ToString() }
            }
        );
    }

  public string GetUserHouseName(int userId)
    {
        string query = "SELECT Houses.name FROM Users INNER JOIN Houses ON Users.house_id = Houses.house_id WHERE User_id = @UserID";
        Dictionary<string, string> queryParams = new()
        {
            { "@UserID", userId.ToString() }
        };
        List<string> result = sqliteOps.SelectQueryWithParams(query, queryParams);
        return result.Count > 0 ? result[0] : "";
    }

    public string GetUserIdByName(string name)
    {
        string query = "SELECT user_id FROM Users WHERE username = @name";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@name", name }
        };
        
        List<string> result = sqliteOps.SelectQueryWithParams(query, queryParams);
        return result.Count > 0 ? result[0] : "";
    }

    public bool CheckIfAdmin(int userId)
    {
        string query = "SELECT is_admin FROM Users WHERE user_id = @userId";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@userId", userId.ToString() }
        };
        List<string> result = sqliteOps.SelectQueryWithParams(query, queryParams);
        if (result[0] == "1")
        {
            return true;
        }

        return false;
    }

    public string GetHouseFounder(string houseName) {
        return GetSingular(houseName, "founder");
    }

    public string GetHouseMascot(string houseName) {
        return GetSingular(houseName, "mascot");
    }

    public string GetHouseDescription(string houseName) {
        return GetSingular(houseName, "description");
    }

    public string[] GetHouseColors(string houseName) {
        return GetMultiple(houseName, "colors");
    }

    public string[] GetHouseTraits(string houseName) {
        return GetMultiple(houseName, "traits");
    }
    
    private string GetSingular(string houseName, string attribute) {
        Dictionary<string, string> parms = new();
        parms.Add("@name", houseName);

        // dangerous :O
        var list = sqliteOps.SelectQueryWithParams("SELECT " + attribute + " FROM Houses WHERE name = @name LIMIT 1",
            parms);
        return list[0];
    }

    // TODO: Make the query return an array or List to begin with
    private string[] GetMultiple(string houseName, string attribute) {
        return GetSingular(houseName, attribute).Split(',');
    }
}