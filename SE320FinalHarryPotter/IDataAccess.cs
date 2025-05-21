namespace SE320FinalHarryPotter;

public interface IDataAccess
{
    public SqliteOps SqliteOps { get; }
    public List<string> GetHouseNames();
    
    public string GetHouseDescription(int houseId);
    
    public void CreateAccount(string username, string password);
    
    public List<string> Login(string username, string password);

    public void SetAdmin(int userID);

    public bool IsUniqueUsername(string username);
}