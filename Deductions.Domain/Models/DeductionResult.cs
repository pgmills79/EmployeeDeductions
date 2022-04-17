using System.Text.Json.Serialization;

namespace Deductions.Domain.Models
{
    public class DeductionResult : IPerson
    {
        public string Name { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Spouse { get; set; }
        
        public string TotalDeductionAmount { get; set; }
        
        public string TotalAmountOfDiscount { get; set; }
        
        public int NumberOfDependents { get; set; }
        
        public string PaycheckAmount { get; set; }
        
       
    }
}