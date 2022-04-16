using System;
using Deductions.Domain.Models;

namespace Deductions.Domain
{

    public interface IUtility
    {
        bool DoesNameStartWithLetter(string nameInput, char letterToCheck);
    }
    public class Utility : IUtility
    {
        public bool DoesNameStartWithLetter(string nameInput, char letterToCheck)
        {
            if (string.IsNullOrEmpty(nameInput)) return false;
            
            return !string.IsNullOrEmpty(letterToCheck.ToString()) 
                   && nameInput.TrimStart().StartsWith(letterToCheck.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}