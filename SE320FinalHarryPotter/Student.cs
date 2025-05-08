using System.Collections.Generic;
namespace SE320FinalHarryPotter;


//house count added 
// might change this class sorry idk
public class Student
{
    public SqliteOps SqliteOps = new SqliteOps();
    
    public int GetStudentCountInHouse(string houseName)
    {
        string query = @"SELECT COUNT(*)
                FROM Users as U 
                INNER JOIN Houses as H 
                    ON U.house_id = H.house_id
                WHERE H.name = @houseName;";

        Dictionary<string, string> queryParams = new Dictionary<string, string>()
        {
            { "@houseName", houseName }
        };
        List<string> count = SqliteOps.SelectQueryWithParams(query, queryParams);
        
        return Int32.Parse(count[0]);
    }
    
}

