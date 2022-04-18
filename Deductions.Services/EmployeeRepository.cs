using System;
using Deductions.Domain;

namespace Deductions.Services
{
    public interface IEmployeeRepository
    {
        decimal GetEmployeeDeductionAmount(string name);
        decimal GetEmployeeDiscountAmount();
        decimal GetEmployeeTotalCostPerPaycheck(decimal employeeDeductionAmount, decimal spouseDeductionAmount, decimal dependentDeductionAmount);
        bool DoesEmployeeGetDiscount(string inputName);
    }
    
    
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {

        public decimal GetEmployeeDeductionAmount(string name) => GetDeductionAmount(name, GetEmployeeDiscountAmount(), GetEmployeeRegularAmount());

        public decimal GetEmployeeTotalCostPerPaycheck(decimal employeeDeductionAmount, decimal spouseDeductionAmount, decimal dependentDeductionAmount)
        {

            var totalDeductionAmount = employeeDeductionAmount + spouseDeductionAmount + dependentDeductionAmount;
            
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
        
        private static decimal GetEmployeeRegularAmount()
        {
            return Convert.ToDecimal(Constants.EmployeeAnnualCost / Constants.NumberOfPaychecks);
        }

        public bool DoesEmployeeGetDiscount(string inputName) => DoesPersonGetDiscount(inputName);

    }
}