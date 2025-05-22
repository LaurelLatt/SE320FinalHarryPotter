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

    public void ShowUserMenu(User user, Admin admin)
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
5. View Admin Menu
6. Exit
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
                case "5":
                    if (admin.IsAdmin(user.UserID))
                    {
                        ShowAdminMenu(user, admin);
                    }

                    break;
                case "6":
                    keepGoing = false;
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
        
        HouseDescriptor descriptor = new HouseDescriptor(new SqlDataAccess(), house);
        
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
        return user.GetUserHouseName(user.UserID);
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
    
    private void ShowAdminMenu(User user, Admin admin)
    {
        string userInput = "0";
        while (userInput != "4")
        { 
            string Menu = @"
        Admin Menu
        ------------------
        1. Add New House
        2. Update House Description
        3. Change a user's house
        4. Exit Admin
        Select option: 
        ";
            Console.WriteLine(Menu);
            userInput = Console.ReadLine();
            Console.WriteLine();
        
            switch (userInput)
            {
                case "1":
                    PerformCreateHouse(admin);
                    break;
                case "2":
                    PerformUpdateHouseDescription(admin);
                    break;
                case "3":
                    PerformChangeUserHouse(user, admin);
                    break;
                default:
                    Console.WriteLine("Please enter a valid option");
                    break;
            }
        }
    }

    private void PerformCreateHouse(Admin admin)
    {
        Console.Write("Enter the name of the house: ");
        string houseName = Console.ReadLine();
        
        Console.Write("Enter the founder of the house: ");
        string founder = Console.ReadLine();

        Console.Write("Enter the mascot of the house: ");
        string mascot = Console.ReadLine();
        
        Console.Write("Enter the colors of the house: ");
        string colors = Console.ReadLine();
        
        Console.Write("Enter the key traits of the house: ");
        string traits = Console.ReadLine();
        
        Console.Write("Enter a short house description: ");
        string description = Console.ReadLine();
        
        admin.CreateHouse(houseName, founder, mascot, colors, traits, description);
    }

    private void PerformUpdateHouseDescription(Admin admin)
    {
        Console.Write("Enter the name of the house: ");
        string houseName = Console.ReadLine();
        
        Console.Write("Enter the new description: ");
        string description = Console.ReadLine();
        
        bool updatedDescription = admin.UpdateHouseDescription(houseName, description);
        if (updatedDescription == false)
        {
            Console.WriteLine("There was an error. Please check the house exists and try again.");
        }
        else
        {
            Console.WriteLine("House updated!");
        }
    }

    private void PerformChangeUserHouse(User user, Admin admin)
    {
        Console.Write("Enter the name of the user: ");
        string userName = Console.ReadLine();
        if (userName == "")
        {
            Console.WriteLine("No user found!");
        }
        else
        {
            int userId = int.Parse(user.GetUserId(userName));
            Console.Write("Enter the new house name for the user:");
            string houseName = Console.ReadLine();
            admin.ChangeUserHouse(userId, houseName);
        }
    }
}