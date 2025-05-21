using System.ComponentModel.Design.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace SE320FinalHarryPotter;

public class User {
    public int UserID;
    
    private IDataAccess dataAccess;
    private SqliteOps ops;

    public User(IDataAccess dataAccess, SqliteOps ops)
    {
        this.dataAccess = dataAccess;
        this.ops = ops;
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
    public virtual void AskAndSetUserHouse(int userId)
    {
        
        // UI
        Console.WriteLine("What house are you in?");
        string input = Console.ReadLine()?.Trim();
        
        //validate the house using housepicked
        var picker = new HousePicker(dataAccess);
        string validHouse = picker.Evaluate(input);

        if (validHouse == "Invalid")
        {
            // UI
            Console.WriteLine("Invalid house. Please try again!");
            return;
        }
        
        //update the user house in database
        
        // databaase
        string query = "UPDATE Users SET house_name = @house WHERE user_id=@userID)";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@house", validHouse },
            { "@userID", userId.ToString() }
        };
        
        ops.ModifyQueryWithParams(query, queryParams);
        
        // UI
        Console.WriteLine($"Your House has been set to: {validHouse}.");
    }
    
    public int GetStudentCountInHouse(string houseName)
    {
        
        // all goes in dataccess
        string query = @"SELECT COUNT(*)
                FROM Users as U 
                INNER JOIN Houses as H 
                    ON U.house_id = H.house_id
                WHERE H.name = @houseName;";

        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@houseName", houseName }
        };
        List<string> count = ops.SelectQueryWithParams(query, queryParams);
        
        return Int32.Parse(count[0]);
    }
    public Dictionary<string, double> GetHouseMembershipPercentages()
    {
        // all goes in data access
        var totalUsersResult = ops.SelectQuery("SELECT COUNT(*) FROM Users");
        int totalUsers = int.Parse(totalUsersResult[0]);

        var houseCounts = ops.SelectQuery("SELECT house_name, COUNT(*) FROM Users GROUP BY house_name");

        var percentages = new Dictionary<string, double>();
        foreach (var row in houseCounts)
        {
            var parts = row.Split(", ");
            string house = parts[0];
            int count = int.Parse(parts[1]);
            percentages[house] = (double)count / totalUsers * 100;
        }

        return percentages;
    }
}