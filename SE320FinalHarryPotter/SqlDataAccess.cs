namespace SE320FinalHarryPotter;

public class SqlDataAccess(SqliteOps sqliteOps) : IDataAccess {
    // To be able to reuse code, pull out database methods so that
    // you can create a method in a new project that manipulates a different database
    
    public SqliteOps SqliteOps { get; private set; } = sqliteOps;

    public List<string> GetHouseNames()
    {
        return SqliteOps.SelectQuery("SELECT name FROM Houses");
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
        SqliteOps.ModifyQueryWithParams(query, queryParams);
    }

    public List<string> Login(string username, string password)
    {
        string query = "SELECT user_id FROM Users WHERE username=@username AND password=@password";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@username", username },
            { "@password", password }
        };
        return SqliteOps.SelectQueryWithParams(query, parameters);
    }

    public void SetAdmin(int userID)
    {
        string query = "UPDATE Users SET is_admin=1 WHERE user_id=@userID";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@userID", userID.ToString() }
        }; 
        SqliteOps.ModifyQueryWithParams(query, parameters);
    }

    public bool IsUniqueUsername(string username)
    {
        string query = "SELECT COUNT(*) FROM Users WHERE username=@username";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@username", username }
        };
        List<string> output = SqliteOps.SelectQueryWithParams(query, parameters);
        return output[0] == "0";
    }
}