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

    private void PerformSetUserHouse(User user)
    {
        // UI
        Console.WriteLine("What house are you in?");
        string input = Console.ReadLine()?.Trim();

        HousePicker picker = new HousePicker(new SqlDataAccess());
        string house = picker.Evaluate(input);

        if (house == "Invalid")
        {
            Console.WriteLine("Invalid House. Try again please.");
            return;
        }
        bool result = user.SetUserHouse(user.UserID, house);
        Console.WriteLine(result ? $"Your house was set to {house}!" : "Failed to set house!");
    }

    public void ShowUserMenu(User user)
    {
        bool keepGoing = true;

        while (keepGoing)
        {
            Console.WriteLine(@"
User Menu:
1. Set House
2. View My House Description
3. View Student Count in My House
4. View House Membership Percentages
5. Exit
Select option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    PerformSetUserHouse(user);
                    break;
                case "2":
                    ShowHouseDetails(user);
                    break;
                case "3":
                    ShowHouseStudentCount(user);
                    break;
                case "4":
                    ShowMembershipPercentages(user);
                    break;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }
        }
    }

    private void ShowHouseDetails(User user)
    {
        string house = GetUserHouseName(user);
        if (string.IsNullOrEmpty(house))
        {
            Console.WriteLine("No house found!");
            return;
        }
        
        HouseDescriptor descriptor = new HouseDescriptor(new SqliteOps(), house);
        
        Console.WriteLine($"Founder: {descriptor.Founder}");
        Console.WriteLine($"Mascot: {descriptor.Mascot}");
        Console.WriteLine($"Colors: {string.Join(", ", descriptor.Colors)}");
        Console.WriteLine($"Traits: {string.Join(", ", descriptor.Traits)}");
        Console.WriteLine($"Description: {descriptor.Description}");

    }

    private void ShowHouseStudentCount(User user)
    {
        string house = GetUserHouseName(user);
        if (string.IsNullOrEmpty(house))
        {
            Console.WriteLine("No house found!");
            return;
        }

        int count = user.GetStudentCountInHouse(house);
        Console.WriteLine($"Student Count: {count}");
    }
    
    private string GetUserHouseName(User user)
    {
        SqliteOps ops = new SqliteOps();
        string query = "SELECT House FROM Users WHERE UserID = @UserID";
        Dictionary<string, string> queryParams = new()
        {
            { "@id", user.UserID.ToString() }
        };
        List<string> result = ops.SelectQueryWithParams(query, queryParams);
        return result.Count > 0 ? result[0] : "";
    }

    private void ShowMembershipPercentages(User user)
    {
        Dictionary<string, double> percentages = user.GetHouseMembershipPercentages();
        
        Console.WriteLine("House Membership Percentages:");
        foreach (var entry in percentages)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }
}