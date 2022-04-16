using System;
using Deductions.Domain;

namespace Deductions.Services
{
    public interface IEmployeeRepository
    {
        decimal GetEmployeeDeductionAmount(string name);

        decimal GetEmployeeTotalCostPerPaycheck();
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
            if (_utilityService.DoesNameStartWithLetter(name, IBaseRepository.ApplyDiscountLetter))
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

        private static decimal GetEmployeeDiscountAmount()
        {
            return Convert.ToDecimal((EmployeeAnnualCost - IBaseRepository.DiscountPercent * EmployeeAnnualCost) /
                                     IBaseRepository.NumberOfPaychecks);
        }

        public decimal GetEmployeeTotalCostPerPaycheck()
        {
            throw new NotImplementedException();
        }

        private static decimal GetEmployeeDeductionAmount()
        {
            return Convert.ToDecimal(EmployeeAnnualCost / IBaseRepository.NumberOfPaychecks);
        }
    }
}