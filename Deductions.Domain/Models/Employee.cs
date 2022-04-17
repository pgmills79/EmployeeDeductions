using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Deductions.Domain.Models
{

    public class Employee : IPerson
    {
        [JsonPropertyName("Employee")]
        public string Name { get; set; }
        
        public string Spouse { get; set; }
        public List<Dependent> Dependents { get; set; }
    }
}