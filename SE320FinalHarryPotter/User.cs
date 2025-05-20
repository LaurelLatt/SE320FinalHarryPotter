using System.ComponentModel.Design.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace SE320FinalHarryPotter;

public class User
{
    public SqliteOps SqliteOps = new SqliteOps();

    public virtual void CreateAccount(string username, string password)
    {
        string query = "INSERT INTO Users(username, password) VALUES (@username, @password)";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@username", username },
            { "@password", password },
        };
        SqliteOps.ModifyQueryWithParams(query, queryParams);
    }

    public virtual int Login(string username, string password)
    {
        string query = "SELECT user_id FROM Users WHERE username=@username AND password=@password";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@username", username },
            { "@password", password }
        };
        List<string> output = SqliteOps.SelectQueryWithParams(query, parameters);
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
        string query = "UPDATE Users SET is_admin=1 WHERE user_id=@userID";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@userID", userID.ToString() }
        };
        SqliteOps.ModifyQueryWithParams(query, parameters);
    }

    public virtual bool IsUniqueUsername(string username)
    {
        string query = "SELECT COUNT(*) FROM Users WHERE username=@username";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@username", username }
        };
        List<string> output = SqliteOps.SelectQueryWithParams(query, parameters);
        return output[0] == "0";
    }
    
    //ask user which house they are in and it updates dataabse
    public virtual void AskAndSetUserHouse(int userId)
    {
        Console.WriteLine("What house are you in?");
        string input = Console.ReadLine()?.Trim();
        
        //validate the house using housepicked
        var picker = new HousePicker(SqliteOps);
        string validHouse = picker.Evaluate(input);

        if (validHouse == "Invalid")
        {
            Console.WriteLine("Invalid house. Please try again!");
            return;
        }
        
        //update the user house in database
        string query = "UPDATE Users SET house_name = @house WHERE user_id=@userID)";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@house", validHouse },
            { "@userID", userId.ToString() }
        };
        
        SqliteOps.ModifyQueryWithParams(query, queryParams);
        Console.WriteLine($"Your House has been set to: {validHouse}.");
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
        List<string> count = SqliteOps.SelectQueryWithParams(query, queryParams);
        
        return Int32.Parse(count[0]);
    }
    public Dictionary<string, double> GetHouseMembershipPercentages()
    {
        var totalUsersResult = SqliteOps.SelectQuery("SELECT COUNT(*) FROM Users");
        int totalUsers = int.Parse(totalUsersResult[0]);

        var houseCounts = SqliteOps.SelectQuery("SELECT house_name, COUNT(*) FROM Users GROUP BY house_name");

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