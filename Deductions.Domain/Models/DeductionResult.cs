using System.Text.Json.Serialization;

namespace Deductions.Domain.Models
{
    public class DeductionResult : IPerson
    {
        [JsonPropertyName("Employee Name")]
        public string Name { get; set; }
        
        [JsonPropertyName("Deduction Amount")]
        public string TotalDeductionAmount { get; set; }
        
        [JsonPropertyName("Discounts Applied")]
        public string TotalAmountOfDiscount { get; set; }
        
        [JsonPropertyName("Number of Dependents")]
        public int NumberOfDependents { get; set; }
        
        [JsonPropertyName("Net Paycheck Amount")]
        public string PaycheckAmount { get; set; }
    }
}