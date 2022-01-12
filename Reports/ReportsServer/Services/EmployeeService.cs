using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using DTO.Exceptions;
using Newtonsoft.Json;

namespace DTO
{
    public class EmployeeService
    {
        public EmployeeService()
        {

            Employees = new List<Employee>();
        }
        private string JsonPath { get; } =@"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Employees.json";
        public List<Employee> Employees { get; set; }
        public const int EmployeesOnPage = 5;

        private void Serialize()
        {
            string json = JsonConvert.SerializeObject(Employees);
            using var streamWriter = new StreamWriter(JsonPath);
            streamWriter.WriteLine(json);
        }

        private List<Employee> Deserialize()
        {
            using var streamReader = new StreamReader(JsonPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Employee>>(json);
        }
        
        public List<Employee> GetAllEmployees()
        {
            Employees = Deserialize();
            return Employees;
        }

        public List<Employee> GetAllEmployeesOnNPage(int numberOfPage)
        {
            Employees = Deserialize();
            int amountOfEmployees = Employees.Count;
            int numberOfPages = amountOfEmployees / EmployeesOnPage + 1;
            int amountOfEmployeesOnLastPage = amountOfEmployees % EmployeesOnPage;
            if (numberOfPage > numberOfPages) throw new WrongPageException();
            return numberOfPage == numberOfPages ? Employees.GetRange((numberOfPage - 1) * EmployeesOnPage, amountOfEmployeesOnLastPage)
                : Employees.GetRange((numberOfPage - 1) * EmployeesOnPage, EmployeesOnPage);
        }

        public Employee GetEmployeeById(Guid id)
        {
            Employees = Deserialize();
            return Employees.FirstOrDefault(employee => employee.Id.Equals(id));
        }

        public Employee GetEmployeeByName(string name)
        {
            Employees = Deserialize();
            return Employees.FirstOrDefault(employee => employee.Name.Equals(name));
        }

        public Employee UpdateLeader(Guid employeeId, Guid leaderId)
        {
            Employees = Deserialize();
            Employee employee = Employees.FirstOrDefault(employee => employee.Id.Equals(employeeId));
            Employee leader = Employees.FirstOrDefault(empl => empl.Id.Equals(leaderId));
            employee.LeaderId = leaderId;
            leader.Subordinates.Add(employee);
            Serialize();
            return employee;
        }

        public void FireEmployee(Guid employeeId)
        {
            Employees = Deserialize();
            Employee employee = GetEmployeeById(employeeId);
            Employee leader = Employees.FirstOrDefault(empl => empl.Id.Equals(employee.LeaderId));
            leader.Subordinates.Remove(employee);
            Employees.Remove(employee);
            Serialize();
        }

        public Employee HireEmployee(string name, Guid leaderId = default)
        {
            Employees = Deserialize();
            var employee = new Employee(name, leaderId);
            Employees.Add(employee);
            Serialize();
            return employee;
        }
    }
}