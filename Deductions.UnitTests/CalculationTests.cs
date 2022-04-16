using System;
using System.Collections.Generic;
using Deductions.Domain;
using Deductions.Services;
using Xunit;

namespace Deductions.UnitTests
{
    public class CalculationTests 
    {
        
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDependentRepository _dependentRepository;
        private readonly IUtility _utility;

        public CalculationTests()
        {
            //The 'DI" is not baked into XUnit and since this is not the integration test just newing these up
            _utility = new Utility();
            _dependentRepository = new DependentRepository(_utility);
            _employeeRepository = new EmployeeRepository(_utility);
        }

        private const int NumberOfPaychecks = 26;
        private const int DependentAnnualCost = 500;
        private const  decimal DiscountPercent = 0.10m;
        
        [Fact]
        public void GetDependentDeductionAmount_Should_Return_Correct_Amount()
        {
            //arrange
            var expectedResult = Convert.ToDecimal(Utility.DependentAnnualCost / Utility.NumberOfPaychecks);
            
            //action
            var actualResult = _dependentRepository.GetDependentDeductionAmount();

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
            var nameStartsWithInputLetter = _utility.DoesNameStartWithLetter(nameInput, letterToCheck);

            //assert
           Assert.True(nameStartsWithInputLetter.Equals(expectedResult));
        }

        #region Dependent Calculations
        [Theory]
        [MemberData(nameof(TestGetDependentsTestData))]
        public void Get_Dependents_Paycheck_Deduction_Amount(List<string> dependents, decimal expectedAmount)
        {
            //arrange

            //action
            var paycheckDeductionAmount = _dependentRepository.GetDependentsPaycheckDeductionAmount(dependents).ToString("#.##");

            //assert
            Assert.True(paycheckDeductionAmount.Equals(expectedAmount.ToString("#.##")));
        }
        
        public static IEnumerable<object[]> TestGetDependentsTestData =>
            new List<object[]>
            {
                //No Discounts for these dependents
                new object[] { new List<string> { "John Doe", "Paul M", "Tom Jerry" }, Convert.ToDecimal(DependentAnnualCost / NumberOfPaychecks) * 3 },
                new object[] { new List<string> { "John Doe", "Paul M" }, Convert.ToDecimal(500 / 26) * 2 },
                //10% discount on 1 dependent
                new object[] { new List<string> { "Adam Bob" }, Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks) * 1 },
                //10% discount on 1 dependent
                new object[] { new List<string> { "Adam Bob", "John Doe" }, Convert.ToDecimal(DependentAnnualCost / NumberOfPaychecks) * 1 + Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks) },
                //10% discount on 2 dependents
                new object[] { new List<string> { "Adam Bob", "Aaron Willis" }, Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks) * 2 },
                
            };
        

        #endregion
        
        #region Employee Calculations
        [Theory]
        [InlineData("John Doe", 38.00)]
        [InlineData("Aaron Lewis", 34.62)]
        [InlineData("  Aaron Lewis", 34.62)]
        [InlineData("", 0.00)]
        public void Get_Employee_Deduction_Amount(string name, decimal expectedAmount)
        {
            //arrange

            //action
            var paycheckDeductionAmount = _employeeRepository.GetEmployeeDeductionAmount(name).ToString("#.##");

            //assert
            Assert.True(paycheckDeductionAmount.Equals(expectedAmount.ToString("#.##")));
        }
        
    
        

        #endregion

       
    }
    
   

       
}
