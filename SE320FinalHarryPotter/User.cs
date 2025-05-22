using System.ComponentModel.Design.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace SE320FinalHarryPotter;

public class User {
    public int UserID;
    
    private IDataAccess dataAccess;

    public User(IDataAccess dataAccess) {
        this.dataAccess = dataAccess;
    }
    
    public virtual void CreateAccount(string username, string password)
    {
        dataAccess.CreateAccount(username, password);
    }

    public virtual int Login(string username, string password)
    {
        List<string> output = dataAccess.Login(username, password);
        
        if (output.IsNullOrEmpty())
        {
            return 0;
        }
        else
        {
            int userID = Int32.Parse(output[0]);
            return userID;
        }
    }

    public virtual void SetAdmin(int userID)
    {
        dataAccess.SetAdmin(userID);
    }

    public virtual bool IsUniqueUsername(string username)
    {
        return dataAccess.IsUniqueUsername(username);
    }
    
    //ask user which house they are in and it updates dataabse
    public virtual bool SetUserHouse(int userId, string houseName)
    {
        //validate the house using housepicked
        HousePicker picker = new HousePicker(dataAccess);
        string validHouse = picker.Evaluate(houseName);

        if (validHouse == "Invalid")
        {
            return false;
        }
        
        string houseId = dataAccess.GetHouseIdByName(validHouse);
        
        if (!string.IsNullOrWhiteSpace(houseId))
        {
            dataAccess.UpdateUserHouse(userId, houseId);
            return true;
        }
        return false;
    }
    
    public int GetStudentCountInHouse(string houseName)
    {
        return dataAccess.GetStudentCountInHouse(houseName);
    }
    public Dictionary<string, double> GetHouseMembershipPercentages()
    {
        int totalUsers = dataAccess.GetTotalStudentCount();

        List<string> houseCounts = dataAccess.GetStudentCountInHouseList();

        Dictionary<string, double> percentages = new Dictionary<string, double>();
        foreach (string row in houseCounts)
        {
            string[] parts = row.Split(", ");
            string house = parts[0];
            int count = int.Parse(parts[1]);
            percentages[house] = (double)count / totalUsers * 100;
        }

        return percentages;
    }
}