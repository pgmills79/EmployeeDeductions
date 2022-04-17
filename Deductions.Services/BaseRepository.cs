﻿using System;
using Deductions.Domain;

namespace Deductions.Services
{
    public abstract class BaseRepository
    {

        protected decimal GetDeductionAmount(string name, decimal discountAmount, decimal regularDeduction)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            return DoesPersonGetDiscount(name) ? discountAmount : regularDeduction;
        }

        protected bool DoesPersonGetDiscount(string nameInput)
        {
            return !string.IsNullOrEmpty(nameInput) && nameInput.TrimStart()
                .StartsWith(Constants.ApplyDiscountLetter, StringComparison.InvariantCultureIgnoreCase);
        }
        
    }
}