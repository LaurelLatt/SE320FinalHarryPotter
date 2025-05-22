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
    
    private IDataAccess dataAccess;

    public Admin(IDataAccess dataAccess) {
        this.dataAccess = dataAccess;
    }
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
    public void CreateHouse(string name, string founder, string mascot, string colors, string traits, string description)
    {
        dataAccess.CreateHouse(name, founder, mascot, colors, traits, description);
        NotifyObserver(name);//observer hook
    }
    
    public List<string> GetHouseList()
    {
        return dataAccess.GetHouseList();
    }
    
    public string GetHouseIdByName(string name)
    {
        return dataAccess.GetHouseIdByName(name);
    }
    
    public bool UpdateHouseDescription(string houseName, string newDescription)
    {
        string houseID = GetHouseIdByName(houseName);
        if (!string.IsNullOrWhiteSpace(houseID))
        {
            dataAccess.UpdateHouseDescription(houseID, newDescription);
            return true;
        }
        return false;
    }
    
    
    public bool ChangeUserHouse(int userId, string newHouseName)
    {
        
        string houseID = GetHouseIdByName(newHouseName);

        if (!string.IsNullOrWhiteSpace(houseID))
        {
            dataAccess.UpdateUserHouse(userId, houseID);
            return true;
        }
        return false;
    }

    public bool IsAdmin(int userId)
    {
        return dataAccess.CheckIfAdmin(userId);
    }
}