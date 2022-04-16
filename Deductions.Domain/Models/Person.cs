namespace Deductions.Domain.Models
{
    public interface IPerson
    {
        public string Name { get; set; }
    }

    public class Person : IPerson
    {
        public string Name { get; set; }
    }
}