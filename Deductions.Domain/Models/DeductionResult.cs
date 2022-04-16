namespace Deductions.Domain.Models
{
    public class DeductionResult : IPerson
    {
        public string Name { get; set; }
        
        public decimal TotalDeductionAmount { get; set; }
        
        public int NumberOfDependents { get; set; }
    }
}