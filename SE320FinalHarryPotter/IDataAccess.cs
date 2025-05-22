namespace SE320FinalHarryPotter;

public interface IDataAccess
{
    
    public List<string> GetHouseNames();
    
    public string GetHouseDescription(int houseId);
    
    public void CreateAccount(string username, string password);
    
    public List<string> Login(string username, string password);

    public void SetAdmin(int userID);

    public bool IsUniqueUsername(string username);
    
    public int GetStudentCountInHouse(string houseName);

    public int GetTotalStudentCount();
    
    public List<string> GetStudentCountInHouseList();

    public void CreateHouse(string name, string founder, string mascot, string colors, string traits, string description);

    public List<string> GetHouseList();
    
    public string GetHouseIdByName(string houseName);

    public void UpdateHouseDescription(string houseId, string description);

    public void UpdateUserHouse(int userId, string newHouseName);

    public string GetUserHouseName(int userId);

    public string GetUserIdByName(string name);
    public bool CheckIfAdmin(int userId);

}
