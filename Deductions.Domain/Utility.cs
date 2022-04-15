using System;
using System.Collections.Generic;
using System.Linq;

namespace Deductions.Domain
{
    public static class Utility
    {
        public const int NumberOfPaychecks = 26;
        public const int DependentAnnualCost = 500;
        private const int EmployeeAnnualCost = 1000;
        private const decimal DiscountPercent = 0.10m;
        private const char ApplyDiscountLetter = 'A';
        
        public static bool DoesNameStartWithLetter(string nameInput, char letterToCheck)
        {
            if (string.IsNullOrEmpty(nameInput)) return false;
            
            return !string.IsNullOrEmpty(letterToCheck.ToString()) 
                   && nameInput.TrimStart().StartsWith(letterToCheck.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static decimal GetDependentsPaycheckDeductionAmount(List<string> dependents)
        {
            var deductionAmount = 0.00m;

            //if no dependents found just return zero
            if (dependents?.Any() != true) return 0;

            //add up the dependents amounts
            foreach (var dependent in dependents)
            {
                if (DoesNameStartWithLetter(dependent, ApplyDiscountLetter))
                {
                    deductionAmount += GetDependentDiscountAmount();
                }
                else
                {
                    deductionAmount += GetDependentDeductionAmount();
                }
            }
            
            //does name get
            return deductionAmount;
        }

        public static decimal GetDependentDeductionAmount()
        {
            return Convert.ToDecimal(DependentAnnualCost / NumberOfPaychecks);
        }

        public static decimal GetDependentDiscountAmount()
        {
            return Convert.ToDecimal( (DependentAnnualCost - DiscountPercent * DependentAnnualCost) / NumberOfPaychecks);
        }
        
        public static decimal GetEmployeeDeductionAmount()
        {
            return Convert.ToDecimal(EmployeeAnnualCost / NumberOfPaychecks);
        }
    }
}