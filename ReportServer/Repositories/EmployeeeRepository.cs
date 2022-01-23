using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO;
using Newtonsoft.Json;

namespace ReportsServer.Repositories
{
    public class EmployeeRepository
    {
        public EmployeeRepository(string employeesPath)
        {
            EmployeesPath = employeesPath;
            Employees = new List<Employee>();
        }
        private string EmployeesPath { get; }
        private List<Employee> Employees { get; set; }

        public void SerializeEmployees()
        {
            string json = JsonConvert.SerializeObject(Employees);
            using var streamWriter = new StreamWriter(EmployeesPath);
            streamWriter.WriteLine(json);
        }
        
        private List<Employee> DeserializeEmployees()
        {
            using var streamReader = new StreamReader(EmployeesPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Employee>>(json);
        }

        public List<Employee> GetAllEmployees()
        {
            return DeserializeEmployees();
        }
        
        public Employee GetEmployeeById(Guid id)
        {
            Employees = DeserializeEmployees();
            return Employees.FirstOrDefault(employee => employee.Id.Equals(id));
        }

        public Employee GetEmployeeByName(string name)
        {
            return GetAllEmployees().FirstOrDefault(employee => employee.Name.Equals(name));
        }

        public Employee GetLeaderByEmployee(Employee employee)
        {
            return GetAllEmployees().FirstOrDefault(empl => empl.Id.Equals(employee.LeaderId));
        }
        public void AddEmployee(Employee employee)
        {
            Employees = DeserializeEmployees();
            Employees.Add(employee);
            SerializeEmployees();
        }

        public void RemoveEmployee(Employee employee)
        {
            Employees = DeserializeEmployees();
            Employees.Remove(Employees.FirstOrDefault(employee1 => employee1.Id.Equals(employee.Id)));
            SerializeEmployees();
        }
    }
}