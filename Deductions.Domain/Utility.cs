using System;

namespace Deductions.Domain
{

    public interface IUtility
    {
        bool DoesNameStartWithLetter(string nameInput, char letterToCheck);
    }
    public class Utility : IUtility
    {
        private const int NumberOfPaychecks = 26;
        private const int EmployeeAnnualCost = 1000;
        
        public bool DoesNameStartWithLetter(string nameInput, char letterToCheck)
        {
            if (string.IsNullOrEmpty(nameInput)) return false;
            
            return !string.IsNullOrEmpty(letterToCheck.ToString()) 
                   && nameInput.TrimStart().StartsWith(letterToCheck.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static decimal GetEmployeeDeductionAmount()
        {
            return Convert.ToDecimal(EmployeeAnnualCost / NumberOfPaychecks);
        }
    }
}