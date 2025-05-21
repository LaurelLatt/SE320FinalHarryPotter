namespace SE320FinalHarryPotter;

//observer pattern
public interface IHouseObserver
{
    void OnHouseCreated(string houseName);
}
public class RandomLoggerObserver : IHouseObserver
{
    public void OnHouseCreated(string houseName)
    {
        Console.WriteLine($"[LOG] House '{houseName}' is now created (observer notification).");
    }
}
public class Admin
{
    public SqliteOps SqliteOps = new SqliteOps();
    //observer pattern support
    private List<IHouseObserver> _observers = new List<IHouseObserver>();
    public void AddObserver(IHouseObserver observer)
    {
        _observers.Add(observer);
    }
    public void NotifyObserver(string houseName)
    {
        foreach (var observer in _observers)
        {
            observer.OnHouseCreated(houseName);
        }
    }
    public void CreateHouse(string name, string founder, string mascot, List<string> colors, List<string> traits, string description)
    {
        
        string query = @"INSERT INTO Houses(name, founder, mascot, colors, traits, description)
                        VALUES (@name, @founder, @mascot, @colors, @traits, @description)
                        ";
        Dictionary<string, string> queryParams = new()
        {
            { "@name", name },
            { "@founder", founder },
            { "@mascot", mascot },
            { "@colors", string.Join(",", colors) },
            { "@traits", string.Join(",", traits) },
            { "@description", description }
        };
        SqliteOps.ModifyQueryWithParams(query, queryParams);
        NotifyObserver(name);//observer hook
    }
    
    public List<string> GetHouseList()
    {
        string query = @"SELECT * FROM Houses";
        return SqliteOps.SelectQuery(query);
        
    }
    
    public string GetHouseByName(string name)
    {
        string query = @"SELECT house_id FROM Houses WHERE name = @name";
        Dictionary<string, string> queryParams = new()
        {
            { "@name", name }
        };
        List<string> houseID= SqliteOps.SelectQueryWithParams(query, queryParams);
        return houseID.Count > 0 ? houseID[0] : "";
    }
    
    public bool UpdateHouseDescription(string houseName, string newDescription)
    {
        string houseID = GetHouseByName(houseName);
        if (!string.IsNullOrWhiteSpace(houseID))
        {
            string query = "UPDATE Houses SET description = @newDescription WHERE house_id = @houseId";
            Dictionary<string, string> queryParams = new()
            {
                { "@newDescription", newDescription },
                { "@houseId", houseID }
            };
            SqliteOps.ModifyQueryWithParams(query, queryParams);
            return true;
        }
        return false;
    }
    
    
    public bool ChangeUserHouse(int userId, string newHouseName)
    {
        
        var houseCheck = SqliteOps.SelectQueryWithParams(
            "SELECT house_id FROM Houses WHERE name = @name",
            new Dictionary<string, string> { { "@name", newHouseName } }
        );
        
        if (houseCheck.Count == 0)
        {
            return false;
        }
        
        
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