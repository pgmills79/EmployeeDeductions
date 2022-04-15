using System;

namespace Deductions.Domain
{
    public static class Utility
    {
        public static bool DoesNameStartWithLetter(string nameInput, char letterToCheck)
        {
            if (string.IsNullOrEmpty(nameInput)) return false;
            
            return !string.IsNullOrEmpty(letterToCheck.ToString()) 
                   && nameInput.TrimStart().StartsWith(letterToCheck.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static decimal GetTotalCostDependents(int numberOfDependents = 0)
        {
            if (numberOfDependents <= 0) return 0;

            return numberOfDependents * 500;
        }
    }
}