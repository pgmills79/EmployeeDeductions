using System;
using Deductions.Domain;

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
            /*
               The regular deduction amount is the annual spouse deduction amount 
               divided by the number of paychecks in a year
           */
            return Convert.ToDecimal(Constants.SpouseAnnualCost / Constants.NumberOfPaychecks);
        }

        public decimal GetSpouseDiscountAmount()
        {
            /*
                Discount amount is taking the percentage off of the annual amount and then subtracting that number 
                from the annual cost.  Since there are 26 paychecks the last step is then to divide by 26
            */
            return Convert.ToDecimal( 
                //#1 Subtract the discount amount from the annual amount
                (Constants.SpouseAnnualCost - Constants.DiscountPercent * Constants.SpouseAnnualCost) 
                //#2 divide by the number of paychecks in the year
                / Constants.NumberOfPaychecks);
        }
        
        public bool DoesSpouseGetDiscount(string inputName) => DoesPersonGetDiscount(inputName);
        
    }
}