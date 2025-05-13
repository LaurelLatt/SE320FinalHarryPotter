namespace SE320FinalHarryPotter;

public class AppFunctions
{
    private User user;
    
    public AppFunctions(User user = null)
    {
        this.user = user ?? new User();
    }

    public int DisplayStartScreen()
    {
        int userID = 0;
        while (userID == 0)
        {
            string menu = @" 
        Welcome! 
        1. Login
        2. Create Account         
        Select option: ";
            Console.Write(menu);
            string userInput = Console.ReadLine();
            Console.WriteLine();
            switch (userInput)
            {
                case "1":
                    userID = Login();
                    break;
                case "2":
                    userID = CreateAccount();
                    break;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }
            
        }
        return userID;
    }

    private int Login()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        int userID = user.Login(username, password);
        return userID;
    }

    private int CreateAccount()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();
        bool uniqueUsername = user.IsUniqueUsername(username);
        while (!uniqueUsername)
        {
            Console.WriteLine("Username already exists!");
            Console.Write("Username: ");
            username = Console.ReadLine();
            uniqueUsername = user.IsUniqueUsername(username);
        }
        Console.Write("Password: ");
        string password = Console.ReadLine();
        user.CreateAccount(username, password);
        int userID = user.Login(username, password);
        return userID;
    }
}