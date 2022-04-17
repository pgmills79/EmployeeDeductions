using System.Text.Json.Serialization;

namespace Deductions.Domain.Models
{
    public class Spouse : IPerson
    {
        
        [JsonPropertyName("Spouse")]
        public string Name { get; set; }

        public Spouse(string name)
        {
            Name = name;
        }
    }
}