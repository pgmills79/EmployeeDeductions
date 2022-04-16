using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Deductions.Domain.Models
{

    public class Employee : IPerson
    {
        [JsonPropertyName("Employee Name")]
        public string Name { get; set; }

        public List<Dependent> Dependents { get; set; }
    }
}