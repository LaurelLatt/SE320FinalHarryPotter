namespace SE320FinalHarryPotter;

public class UI
{
    // Do Singleton Pattern 
    private static UI instance;

    private UI() { }
    
    // Singleton Pattern, can access code anywhere in codebase without having to pass it around,
    // ensures you won't make more than one of them
    public static UI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UI();
            }
            return instance;
        }
    }

    public int getUserID(User user)
    {
        int userID = 0;
        while (userID == 0)
        {
            string LoginMenu = @" 
        Welcome! 
        1. Login
        2. Create Account         
        Select option: ";
            Console.Write(LoginMenu);
            string userInput = Console.ReadLine();
            Console.WriteLine();
            switch (userInput)
            {
                case "1":
                    userID = PerformLogin(user);
                    break;
                case "2":
                    userID = PerformCreateAccount(user);
                    break;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }
            
        }
        return userID;
    }

    private int PerformCreateAccount(User user)
    {
        int userID;
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
        userID = user.Login(username, password);
        return userID;
    }

    private int PerformLogin(User user)
    {
        int userID;
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        userID = user.Login(username, password);
        return userID;
    }
}