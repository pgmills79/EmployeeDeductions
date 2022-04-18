using System;
using Deductions.Domain;

namespace Deductions.Services
{
    public abstract class BaseRepository
    {

        /// <summary>
        /// Method to get the deduction amount for a person
        /// </summary>
        /// <param name="name">The name of the person</param>
        /// <param name="discountAmount">What the discounted deduction amount will be if they qualify for a discount</param>
        /// <param name="regularDeduction">What the regular deduction amount will be if they DO NOT qualify for a discount</param>
        /// <returns></returns>
        protected static decimal GetDeductionAmount(string name, decimal discountAmount, decimal regularDeduction)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            return DoesPersonGetDiscount(name) ? discountAmount : regularDeduction;
        }

        /// <summary>
        /// Method to determine whether or not a person qualifies for a discount on their deduction
        /// </summary>
        /// <param name="nameInput">The name of the person</param>
        /// <returns></returns>
        protected static bool DoesPersonGetDiscount(string nameInput)
        {
            return !string.IsNullOrEmpty(nameInput) && nameInput.TrimStart()
                .StartsWith(Constants.ApplyDiscountLetter, StringComparison.InvariantCultureIgnoreCase);
        }
        
    }
}