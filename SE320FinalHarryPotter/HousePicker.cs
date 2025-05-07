using System.Collections.Generic;

namespace PotterFinal
{
    public class HousePicker
    {
        private readonly HashSet<string> validHouses = new()
        {
            "Ravenclaw", "Slytherin", "Hufflepuff", "Gryffindor"
        };

        public string Evaluate(string userInput)
        {
            return validHouses.Contains(userInput) ? userInput : "Invalid";
        }
    }
}