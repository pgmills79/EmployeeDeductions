using System;
using Deductions.Domain;

namespace Deductions.Services
{
    public interface IEmployeeRepository
    {
        decimal GetEmployeeDeductionAmount(string name);
        decimal GetEmployeeDiscountAmount();
        decimal GetEmployeeTotalCostPerPaycheck(decimal employeeDeductionAmount, decimal dependentDeductionAmount);
    }
    
    
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {

        public EmployeeRepository(IUtility utilityService) : base(utilityService)
        {
        }
        
        public decimal GetEmployeeDeductionAmount(string name) => GetDeductionAmount(name, GetEmployeeDiscountAmount(), GetDeductionAmount());

        public decimal GetEmployeeTotalCostPerPaycheck(decimal employeeDeductionAmount, decimal dependentDeductionAmount)
        {

            var totalDeductionAmount = employeeDeductionAmount + dependentDeductionAmount;
            
            //if the amount is higher than the maximum cost for employee benefits, set it to the maximum amount
            if (totalDeductionAmount > Constants.MaximumDeductionAmount)
                totalDeductionAmount = Constants.MaximumDeductionAmount;

            return totalDeductionAmount;
        }
        
        public decimal GetEmployeeDiscountAmount()
        {
            return Convert.ToDecimal((Constants.EmployeeAnnualCost - Constants.DiscountPercent * Constants.EmployeeAnnualCost) /
                                     Constants.NumberOfPaychecks);
        }
        
        private static decimal GetDeductionAmount()
        {
            return Convert.ToDecimal(Constants.EmployeeAnnualCost / Constants.NumberOfPaychecks);
        }

       
    }
}