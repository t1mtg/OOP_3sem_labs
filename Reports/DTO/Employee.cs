using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DTO
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LeaderId { get; set; }

        public Employee()
        {
        }

        public Employee(string name, Guid leaderId = default)
        {
            Id = Guid.NewGuid();
            Name = name;
            Subordinates = new List<Employee>();
        }
        public List<Employee> Subordinates { get; set; }
    }
}