using System.Collections.Generic;
namespace PotterFinal;
public class House
{
    public SqliteOps SqliteOps = new SqliteOps();
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

public class AdminCreateHouse
{
    public SqliteOps SqliteOps = new SqliteOps();
    
    public void CreateHouse(string name, string founder, string mascot, List<string> colors, List<string> traits, string description)
    {

        string query = @"INSERT INTO Houses(name, founder, mascot, colors, traits, description)
                        VALUES (@name, @founder, @mascot, @colors, @traits, @description)
                        ";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@name", name },
            { "@founder", founder },
            { "@mascot", mascot },
            { "@colors", string.Join(",", colors) },
            { "@traits", string.Join(",", traits) },
            { "@description", description }
        };
        SqliteOps.ModifyQueryWithParams(query, queryParams);
    }
    
    public List<string> GetHouseList()
    {
        string query = @"SELECT * FROM Houses";
        List<string> houses = SqliteOps.SelectQuery(query);
        return houses;
    }
    
    public string GetHouseByName(string name)
    {
        string query = @"SELECT house_id FROM Houses WHERE name = @name";
        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@name", name }
        };
        List<string> houseID= SqliteOps.SelectQueryWithParams(query, queryParams);
        return houseID[0];
    }
}

//new class update house description
public class AdminHouseDescription
{
    public SqliteOps SqliteOps = new SqliteOps();
    private AdminCreateHouse _houseDataAccess;

    public AdminHouseDescription(AdminCreateHouse houseDataAccess)
    {
        _houseDataAccess = houseDataAccess;
    }
    
    //allow admin update description of existing house
    public bool UpdateHouseDescription(string houseName, string newDescription)
    {
        string houseID = _houseDataAccess.GetHouseByName(houseName);
        if (houseID != null)
        {
           string query = "UPDATE Houses SET description = @newDescription WHERE house_id = @houseId";
           Dictionary<string, string> queryParams = new Dictionary<string, string>()
           {
               { "@newDescription", newDescription },
               { "@houseId", houseID }
           };
           SqliteOps.ModifyQueryWithParams(query, queryParams);
           return true;
        }

        return false; //house not found
    }
}