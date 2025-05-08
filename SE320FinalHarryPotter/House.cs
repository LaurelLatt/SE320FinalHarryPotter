namespace SE320FinalHarryPotter;

// im not sure we actually need this class, or we need to change it i haven't decided
public class House
{
    private SqliteOps SqliteOps = new SqliteOps();
    public string Name { get; set; }
    public string Founder { get; set; }
    public string Mascot { get; set; }
    public List<string> Colors { get; set; }
    public List<string> Traits { get; set; }
    
    //added description
    public string Description { get; set; }

    public House(string name, string founder, string mascot, List<string> colors, List<string> traits, string description)
    {
        Name = name;
        Founder = founder;
        Mascot = mascot;
        Colors = colors;
        Traits = traits;
        Description = description; //added
    }
    
}