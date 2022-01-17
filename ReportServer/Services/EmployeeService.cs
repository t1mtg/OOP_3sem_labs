using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using DTO.Exceptions;
using Newtonsoft.Json;
using ReportsServer.Repositories;

namespace DTO
{
    public class EmployeeService
    {
        public EmployeeService(EmployeeRepository repository)
        {
            _employeeRepository = repository;
        }
        private string JsonPath { get; } =@"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Employees.json";
        private readonly EmployeeRepository _employeeRepository;
        private const int EmployeesOnPage = 5;
        
        
        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public Employee GetEmployeeById(Guid employeeId)
        {
            return _employeeRepository.GetEmployeeById(employeeId);
        }

        public List<Employee> GetAllEmployeesOnNPage(int numberOfPage)
        {
            int amountOfEmployees = _employeeRepository.GetAllEmployees().Count;
            int numberOfPages = amountOfEmployees / EmployeesOnPage + 1;
            int amountOfEmployeesOnLastPage = amountOfEmployees % EmployeesOnPage;
            if (numberOfPage > numberOfPages) throw new WrongPageException();
            return numberOfPage == numberOfPages ? _employeeRepository.GetAllEmployees().GetRange((numberOfPage - 1) * EmployeesOnPage, amountOfEmployeesOnLastPage)
                : _employeeRepository.GetAllEmployees().GetRange((numberOfPage - 1) * EmployeesOnPage, EmployeesOnPage);
        }

        public Employee GetEmployeeByName(string name)
        {
            return _employeeRepository.GetEmployeeByName(name);
        }

        public Employee UpdateLeader(Guid employeeId, Guid leaderId)
        {
            Employee employee = _employeeRepository.GetEmployeeById(employeeId);
            employee.LeaderId = leaderId;
            _employeeRepository.SerializeEmployees();
            Employee leader = _employeeRepository.GetEmployeeById(leaderId);
            leader.Subordinates.Add(employee);
            _employeeRepository.SerializeEmployees();
            return employee;
        }

        public void FireEmployee(Guid employeeId)
        {
            Employee employee = _employeeRepository.GetEmployeeById(employeeId);
            Employee leader = _employeeRepository.GetLeaderByEmployee(employee);
            leader?.RemoveSubordinateById(employeeId);
            _employeeRepository.SerializeEmployees();
            _employeeRepository.RemoveEmployee(employee);
        }

        public Employee HireEmployee(string name, Guid leaderId = default)
        {
            var employee = new Employee(name, leaderId);
            _employeeRepository.AddEmployee(employee);
            return employee;
        }
    }
}