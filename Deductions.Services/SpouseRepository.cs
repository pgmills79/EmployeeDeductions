using System;
using System.Collections.Generic;
using System.Linq;
using Deductions.Domain;
using Deductions.Domain.Models;

namespace Deductions.Services
{
    public interface ISpouseRepository
    {
        decimal GetSpouseDeductionAmount(string inputName);

        decimal GetSpouseDiscountAmount();
        
        bool DoesSpouseGetDiscount(string inputName);
    }
    
    public class SpouseRepository : BaseRepository, ISpouseRepository
    {

        public decimal GetSpouseDeductionAmount(string spouseName)
        {
            return string.IsNullOrEmpty(spouseName) ? 0 : GetDeductionAmount(spouseName, GetSpouseDiscountAmount(), GetSpouseRegularAmount());
        }
        private static decimal GetSpouseRegularAmount()
        {
            return Convert.ToDecimal(Constants.SpouseAnnualCost / Constants.NumberOfPaychecks);
        }

        public decimal GetSpouseDiscountAmount()
        {
            return Convert.ToDecimal( (Constants.SpouseAnnualCost - Constants.DiscountPercent * Constants.SpouseAnnualCost) / Constants.NumberOfPaychecks);
        }
        
        public bool DoesSpouseGetDiscount(string inputName) => DoesPersonGetDiscount(inputName);
        
    }
}