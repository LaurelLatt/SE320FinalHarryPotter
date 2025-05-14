namespace SE320FinalHarryPotter;

class Program
{
    static void Main(string[] args)
    {
        User user = new User();
        
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
                    Console.Write("Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();
                    userID = user.Login(username, password);
                    break;
                case "2":
                    Console.Write("Username: ");
                    username = Console.ReadLine();
                    bool uniqueUsername = user.IsUniqueUsername(username);
                    while (!uniqueUsername)
                    {
                        Console.WriteLine("Username already exists!");
                        Console.Write("Username: ");
                        username = Console.ReadLine();
                        uniqueUsername = user.IsUniqueUsername(username);
                    }
                    Console.Write("Password: ");
                    password = Console.ReadLine();
                    user.CreateAccount(username, password);
                    userID = user.Login(username, password);
                    break;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }
            
        }
        
    }
    
}