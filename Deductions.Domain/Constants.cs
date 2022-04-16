namespace Deductions.Domain
{
    public static class Constants
    {
        public const decimal DiscountPercent = 0.10m;
        public const char ApplyDiscountLetter = 'A';
        public const int NumberOfPaychecks = 26;
        
        //making this $2,000 since employees are paid 2,000 we cant deduct more than their paycheck
        public const decimal MaximumDeductionAmount = 2000.00m; 
        
        public const int DependentAnnualCost = 500;
        public const int EmployeeAnnualCost = 1000;
    }
}