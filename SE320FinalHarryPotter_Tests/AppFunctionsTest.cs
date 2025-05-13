using SE320FinalHarryPotter;
namespace SE320FinalHarryPotter_Tests;

public class AppFunctionsTest
{
    
    [Theory]
    [InlineData("1\nharry\npotter123\n", 1)]
    [InlineData("1\nhermione\ngranger456\n1\nron\nweasley123\n", 2)]
    public void DisplayStartScreenLogin(string readerInput, int expectedUserID)
    {
        StringReader input = new StringReader(readerInput);
        Console.SetIn(input);
        Console.SetOut(new StringWriter());

        MockUser mockUser = new MockUser();
        AppFunctions app = new AppFunctions(mockUser);

        int result = app.DisplayStartScreen();

        Assert.Equal(expectedUserID, result);
    }
    
    [Theory]
    [InlineData("2\nharry\npotter123\n", 1)]
    [InlineData("2\nneville\nneville1\nherbologyRox\n", 3)]
    public void DisplayStartScreenCreateAccount(string readerInput, int expectedUserID)
    {
        StringReader input = new StringReader(readerInput);
        Console.SetIn(input);
        Console.SetOut(new StringWriter());

        MockUser mockUser = new MockUser();
        AppFunctions app = new AppFunctions(mockUser);

        int result = app.DisplayStartScreen();

        Assert.Equal(expectedUserID, result);
    }
    
    private class MockUser : User
    {
        private readonly HashSet<string> _existingUsernames = new HashSet<string> { "harry" };
        public override int Login(string username, string password)
        {
            if (username == "harry" && password == "potter123")
            {
                return 1;
            }
            else if (username == "ron" && password == "weasley123")
            {
                return 2;
            }
            else if (username == "neville1" && password == "herbologyRox")
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        public override bool IsUniqueUsername(string username)
        {
            return (username != "neville");
        }
        public override void CreateAccount(string username, string password) { }
    }
}