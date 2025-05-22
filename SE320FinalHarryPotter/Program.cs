using SQLitePCL;

namespace SE320FinalHarryPotter;

class Program
{
    static void Main(string[] args)
    {
        // Make a UI Class that implements it, Program should be 2 lines to just run it
        Console.WriteLine(Directory.GetCurrentDirectory());
        SqliteOps ops = new ();
        User user = new User(new SqlDataAccess());
        
        Login(user);
    }

    private static void Login(User user)
    // Purpose is to get a UserID
    {
        // Follows Singleton
        int userID = UI.Instance.getUserID(user); 
        user.UserID = userID;
    }
}