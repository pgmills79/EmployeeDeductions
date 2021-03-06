using System.Text.Json.Serialization;

namespace Deductions.Domain.Models
{
    public class Dependent : IPerson
    {
        
        [JsonPropertyName("Dependent")]
        public string Name { get; set; }

        public Dependent(string name)
        {
            Name = name;
        }
    }
}