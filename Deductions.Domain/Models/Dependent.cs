using System.Collections.Generic;

namespace Deductions.Domain.Models
{
    public class Dependent : IPerson
    {
        public string Name { get; set; }

        public Dependent(string name)
        {
            Name = name;
        }
    }
}