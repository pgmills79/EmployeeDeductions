using System;
using Deductions.Domain;
using Xunit;

namespace Deductions.UnitTests
{
    public class CalculationTests
    {
        [Theory]
        [InlineData("John Doe", 'A', false)]
        [InlineData("Aaron Mills", 'A', true)]
        [InlineData("Aaron Mills", 'B', false)]
        [InlineData("Paul Mills", 'P', true)]
        [InlineData("Paul Mills", 'p', true)]
        [InlineData("john Doe", 'J', true)]
        [InlineData(null, 'J', false)]
        [InlineData("Paul Mills", ' ', false)]
        [InlineData("  Paul Mills", 'P', true)]
        public void Does_Name_Start_With_Specific_Letter(string nameInput, char letterToCheck, bool expectedResult)
        {
            //arrange

            //action
            var nameStartsWithInputLetter = CommonMethods.DoesNameStartWithLetter(nameInput, letterToCheck);

            //assert
           Assert.True(nameStartsWithInputLetter == expectedResult);
        }
    }
}