using System;

namespace Deductions.Domain
{
    public static class CommonMethods
    {
        public static bool DoesNameStartWithLetter(string nameInput, char letterToCheck)
        {
            if (string.IsNullOrEmpty(nameInput)) return false;
            
            return !string.IsNullOrEmpty(letterToCheck.ToString()) 
                   && nameInput.TrimStart().StartsWith(letterToCheck.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}