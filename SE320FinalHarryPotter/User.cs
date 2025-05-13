using Microsoft.IdentityModel.Tokens;

namespace SE320FinalHarryPotter;

public class User
{
    public SqliteOps SqliteOps = new SqliteOps();

    public void CreateAccount(string username, string password, string house_id)
    {
        string query = "INSERT INTO Users(username, password, house_id) VALUES (@username, @password, @house_id)";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@username", username },
            { "@password", password },
            { "@house_id", house_id }
        };
        SqliteOps.ModifyQueryWithParams(query, queryParams);
    }

    public int Login(string username, string password)
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

    public void SetAdmin(int userID)
    {
        string query = "UPDATE Users SET is_admin=1 WHERE user_id=@userID";
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "@userID", userID.ToString() }
        };
        SqliteOps.ModifyQueryWithParams(query, parameters);
    }
}