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
        if (output[0] == "0")
        {
            return true;
        }
        return false;
    }
}