using System.Collections.Generic;

namespace Deductions.Domain.Models
{
    public interface IEmployee
    {
        List<IPerson> Dependents { get; set; }
    }

    public class Employee : IPerson, IEmployee
    {
        public string Name { get; set; }

        public List<IPerson> Dependents { get; set; }
    }
}