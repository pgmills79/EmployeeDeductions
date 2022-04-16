using System;
using Deductions.Domain;
using Deductions.Domain.Models;

namespace Deductions.Services
{
    public interface IEmployeeRepository
    {
        decimal GetEmployeeDeductionAmount(string name);
        decimal GetEmployeeDiscountAmount();
        decimal GetEmployeeTotalCostPerPaycheck(decimal employeeDeductionAmount, decimal dependentDeductionAmount);
    }
    
    
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IUtility _utilityService;
        private const int EmployeeAnnualCost = 1000;

        public EmployeeRepository(IUtility utilityService)
        {
            _utilityService = utilityService;
        }

        public decimal GetEmployeeDeductionAmount(string name)
        {
            var deductionAmount = 0.00m;

            //if no employee name just return zero
            if (string.IsNullOrEmpty(name)) return 0;

            //add up the dependents amounts
            if (_utilityService.DoesNameStartWithLetter(name, BaseRepository.ApplyDiscountLetter))
            {
                deductionAmount += GetEmployeeDiscountAmount();
            }
            else
            {
                deductionAmount += GetEmployeeDeductionAmount();
            }

            //does name get
            return deductionAmount;
        }

        public decimal GetEmployeeDiscountAmount()
        {
            return Convert.ToDecimal((EmployeeAnnualCost - BaseRepository.DiscountPercent * EmployeeAnnualCost) /
                                     BaseRepository.NumberOfPaychecks);
        }

        public decimal GetEmployeeTotalCostPerPaycheck(decimal employeeDeductionAmount, decimal dependentDeductionAmount)
        {

            var totalDeductionAmount = employeeDeductionAmount + dependentDeductionAmount;
            
            //if the amount is higher than the maximum cost for employee benefits, set it to the maximum amount
            //if (totalDeductionAmount > _utilityService)

            throw new NotImplementedException();
        }

        private static decimal GetEmployeeDeductionAmount()
        {
            return Convert.ToDecimal(EmployeeAnnualCost / BaseRepository.NumberOfPaychecks);
        }
    }
}