using System.Collections.Generic;

namespace RoslynDumper.Tests.Data
{
    public class Organization
    {
        public Organization()
        {
            Persons = new HashSet<Person>();
        }

        public string Name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}