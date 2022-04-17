using System;
using System.Collections.Generic;
using Deductions.Domain;
using Deductions.Domain.Models;
using Deductions.Services;
using Xunit;

namespace Deductions.UnitTests
{
    public class CalculationTests 
    {
        
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDependentRepository _dependentRepository;
        private readonly ISpouseRepository _spouseRepository;

        public CalculationTests()
        {
            
            //The 'DI" is not baked into XUnit and since this is not the integration test just newing these up
            _employeeRepository = new EmployeeRepository();
            _spouseRepository = new SpouseRepository();
            _dependentRepository = new DependentRepository();
            
        }

        private const int NumberOfPaychecks = 26;
        private const int DependentAnnualCost = 500;
        private const  decimal DiscountPercent = 0.10m;


        [Theory]
        [InlineData("John Doe")]
        [InlineData("Aaron Mills")]
        [InlineData("Paul Mills")]
        [InlineData(" Paul Mills")]
        [InlineData("john Doe")]
        [InlineData(null)]
        [InlineData("  Paul Mills")]
        public void Does_Employee_GetDiscount_Should_Return_Correct_Result(string nameInput)
        {
            //arrange
            var expectedResult = !string.IsNullOrEmpty(nameInput) && nameInput.TrimStart()
                .StartsWith(Constants.ApplyDiscountLetter, StringComparison.InvariantCultureIgnoreCase);

            //action
            var result = _employeeRepository.DoesEmployeeGetDiscount(nameInput);

            //assert
           Assert.True(result.Equals(expectedResult));
        }

        #region Dependent/Spouse Tests
        
        [Theory]
        [InlineData("John Doe")]
        [InlineData("Aaron Mills")]
        [InlineData("Paul Mills")]
        [InlineData(" Paul Mills")]
        [InlineData("john Doe")]
        [InlineData(null)]
        [InlineData("  Paul Mills")]
        public void Does_Dependent_GetDiscount_Should_Return_Correct_Result(string nameInput)
        {
            //arrange
            var expectedResult = !string.IsNullOrEmpty(nameInput) && nameInput.TrimStart()
                .StartsWith(Constants.ApplyDiscountLetter, StringComparison.InvariantCultureIgnoreCase);

            //action
            var result = _dependentRepository.DoesDependentGetDiscount(nameInput);

            //assert
            Assert.True(result.Equals(expectedResult));
        }
        
        [Theory]
        [InlineData("John Doe")]
        [InlineData("Aaron Mills")]
        [InlineData("Paul Mills")]
        [InlineData(" Paul Mills")]
        [InlineData("john Doe")]
        [InlineData(null)]
        [InlineData("  Paul Mills")]
        public void Does_Spouse_GetDiscount_Should_Return_Correct_Result(string nameInput)
        {
            //arrange
            var expectedResult = !string.IsNullOrEmpty(nameInput) && nameInput.TrimStart()
                .StartsWith(Constants.ApplyDiscountLetter, StringComparison.InvariantCultureIgnoreCase);

            //action
            var result = _spouseRepository.DoesSpouseGetDiscount(nameInput);

            //assert
            Assert.True(result.Equals(expectedResult));
        }

        [Theory]
        [MemberData(nameof(TestGetDependentsTestData))]
        public void Get_Dependents_Paycheck_Deduction_Amount(List<Dependent> dependents, decimal expectedAmount)
        {
            //arrange

            //action
            var paycheckDeductionAmount = _dependentRepository.GetDependentDeductionAmount(dependents).ToString("#.##");

            //assert
            Assert.True(paycheckDeductionAmount.Equals(expectedAmount.ToString("#.##")));
        }
        
        public static IEnumerable<object[]> TestGetDependentsTestData =>
            new List<object[]>
            {
                //No Discounts for these dependents
                new object[] { new List<Dependent> { new("John Doe"), new ("Paul M"), new ("Tom Jerry")}, Convert.ToDecimal(DependentAnnualCost / NumberOfPaychecks) * 3 },
                new object[] { new List<Dependent> { new("John Doe"), new ("Paul M")}, Convert.ToDecimal(500 / 26) * 2 },
                //10% discount on 1 dependent
                new object[] { new List<Dependent> { new("Adam Bob")}, Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks) * 1 },
                //10% discount on 1 dependent
                new object[] { new List<Dependent> { new("Adam Bob"), new ("John Doe")}, Convert.ToDecimal(DependentAnnualCost / NumberOfPaychecks) * 1 + Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks) },
                //10% discount on 2 dependents
                new object[] { new List<Dependent> { new("Adam Bob"), new ("Aaron Willis")}, Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks) * 2 },
                
            };
        

        #endregion
        
        #region Employee Calculations
        [Theory]
        /*[InlineData("John Doe", 38.00)]
        [InlineData("Aaron Lewis", 34.62)]
        [InlineData("  Aaron Lewis", 34.62)]*/
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
