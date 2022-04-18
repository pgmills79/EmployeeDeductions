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
            
            //if the amount is higher than the maximum cost for employee benefits, set it to the maximum deduction amount
            if (totalDeductionAmount > Constants.MaximumDeductionAmount)
                totalDeductionAmount = Constants.MaximumDeductionAmount;

            return totalDeductionAmount;
        }
        
        public decimal GetEmployeeDiscountAmount()
        {
            /*
               Discount amount is taking the percentage off of the annual amount and then subtracting that number 
               from the annual cost.  Since there are 26 paychecks the last step is then to divide by 26
           */
            return Convert.ToDecimal(
                //#1 Subtract the discount amount from the annual amount
                (Constants.EmployeeAnnualCost - Constants.DiscountPercent * Constants.EmployeeAnnualCost) 
                //#2 divide by the number of paychecks in the year
                / Constants.NumberOfPaychecks);
        }
        
        private static decimal GetEmployeeRegularAmount()
        {
            /*
               The regular deduction amount is the annual employee deduction amount 
               divided by the number of paychecks in a year
           */
            return Convert.ToDecimal(Constants.EmployeeAnnualCost / Constants.NumberOfPaychecks);
        }

        public bool DoesEmployeeGetDiscount(string inputName) => DoesPersonGetDiscount(inputName);

    }
}