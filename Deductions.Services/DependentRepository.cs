using System;
using System.Collections.Generic;
using System.Linq;
using Deductions.Domain;
using Deductions.Domain.Models;

namespace Deductions.Services
{
    public interface IDependentRepository
    {
        decimal GetTotalDependentDiscountAmount(List<Dependent> dependents);
        decimal GetDependentDeductionAmount(List<Dependent> dependents);
        bool DoesDependentGetDiscount(string inputName);
    }
    
    public class DependentRepository : BaseRepository, IDependentRepository
    {

        public decimal GetDependentDeductionAmount(List<Dependent> dependents)
        {
            return dependents?.Any() != true ? 0 : 
                dependents.Sum(dependent => GetDeductionAmount(dependent.Name, GetDependentDiscountAmount(), GetDeductionAmount()));
        }

        public decimal GetTotalDependentDiscountAmount(List<Dependent> dependents)
        {
            if (dependents?.Any() != true) return 0;
            return dependents.Sum(dependent => DoesPersonGetDiscount(dependent.Name)
                ? GetDependentDiscountAmount() : 0);
        }
        
        private static decimal GetDeductionAmount() 
        {
            /*
               The regular deduction amount is the annual employee deduction amount 
               divided by the number of paychecks in a year
           */
            return Convert.ToDecimal(Constants.DependentAnnualCost / Constants.NumberOfPaychecks);
        }

        private static decimal GetDependentDiscountAmount()
        {
            /*
                   Discount amount is taking the percentage off of the annual amount and then subtracting that number 
                   from the annual cost.  Since there are 26 paychecks the last step is then to divide by 26
            */
            return Convert.ToDecimal(
                //#1 Subtract the discount amount from the annual amount
                (Constants.DependentAnnualCost - Constants.DiscountPercent * Constants.DependentAnnualCost) 
                //#2 divide by the number of paychecks in the year
                / Constants.NumberOfPaychecks);
        }
        
        public bool DoesDependentGetDiscount(string inputName) => DoesPersonGetDiscount(inputName);
        
    }
}