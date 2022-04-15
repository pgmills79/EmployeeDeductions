using System;
using System.Collections.Generic;
using Deductions.Domain;
using Xunit;

namespace Deductions.UnitTests
{
    public class CalculationTests
    {
        [Fact]
        public void GetDependentDeductionAmount_Should_Return_Correct_Amount()
        {
            //arrange
            var expectedResult = Convert.ToDecimal(Utility.DependentAnnualCost / Utility.NumberOfPaychecks);
            
            //action
            var actualResult = Utility.GetDependentDeductionAmount();

            //assert
            Assert.True(expectedResult.Equals(actualResult));
        }
        
        
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
            var nameStartsWithInputLetter = Utility.DoesNameStartWithLetter(nameInput, letterToCheck);

            //assert
           Assert.True(nameStartsWithInputLetter.Equals(expectedResult));
        }

        [Theory]
        [MemberData(nameof(TestGetDependentsTestData))]
        public void Get_Dependents_Paycheck_Deduction_Amount(List<string> dependents, decimal expectedAmount)
        {
            //arrange

            //action
            var paycheckDeductionAmount = Utility.GetDependentsPaycheckDeductionAmount(dependents);

            //assert
            Assert.True(paycheckDeductionAmount.Equals(expectedAmount));
        }
        
        public static IEnumerable<object[]> TestGetDependentsTestData =>
            new List<object[]>
            {
                //No Discounts for these dependents
                new object[] { new List<string> { "John Doe", "Paul M", "Tom Jerry" }, Utility.GetDependentDeductionAmount() * 3 },
                new object[] { new List<string> { "John Doe", "Paul M" }, Utility.GetDependentDeductionAmount() * 2 },
                //10% discount on 1 dependent
                new object[] { new List<string> { "Adam Bob" }, Utility.GetDependentDiscountAmount() * 1 },
                //10% discount on 1 dependent
                new object[] { new List<string> { "Adam Bob", "John Doe" }, Utility.GetDependentDiscountAmount() * 1 + Utility.GetDependentDeductionAmount() },
                //10% discount on 2 dependents
                new object[] { new List<string> { "Adam Bob", "Aaron Willis" }, Utility.GetDependentDiscountAmount() * 2 },
                
            };
    }
}