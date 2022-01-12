using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DTO
{
    public interface IEmployeeService
    {
        public List<Employee> GetAllEmployees();

        public Employee GetEmployeeById(Guid id);
        public Employee GetEmployeeByName(string name);

        public Employee UpdateLeader(Guid employeeId, Guid leaderId);

        public void FireEmployee(Guid employeeId);

        public Employee HireEmployee(string name, Guid leaderId = default);
    }
}