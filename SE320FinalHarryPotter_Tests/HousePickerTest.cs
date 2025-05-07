using PotterFinal;
using Xunit;

namespace PotterFinalTest
{
    public class HousePickerTest
    {
        private HousePicker picker = new HousePicker();

        [Theory]
        [InlineData("Ravenclaw")]
        [InlineData("Slytherin")]
        [InlineData("Hufflepuff")]
        [InlineData("Gryffindor")]
        public void UserGetHouseCorrectResult(string userInputHouseName)
        {
            string result = picker.Evaluate(userInputHouseName);
            Assert.Equal(userInputHouseName, result);
        }

        [Fact]
        public void InvalidHouseReturnsInvalid()
        {
            string result = picker.Evaluate("Durmstrang");
            Assert.Equal("Invalid", result);
        }
    }
}