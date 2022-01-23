using System;
using System.Collections.Generic;

namespace DTO
{
    public class WeeklyReport : Report
    {
        public WeeklyReport(Employee employee)
            : base(employee)
        {
            _employees = new List<Employee>();
            _reports = new List<Report>();
            _tasks = new List<Task>();
        }

        public WeeklyReport()
        {
        }

        public List<Employee> _employees;
        public List<Task> _tasks;
        public List<Report> _reports;

        public void AddDailyReports(List<Report> reports)
        {
            _reports.AddRange(reports);
        }

        public void AddTasks(List<Task> tasks)
        {
            _tasks.AddRange(tasks);
        }

        public void AddEmployees(Employee employee)
        {
            _employees.Add(employee);
        }
        
        public List<Report> GetDailyReports()
        {
            return _reports;
        }

        public List<Task> GetTasks()
        {
            return _tasks;
        }

        public List<Employee> GetEmployees()
        {
            return _employees;
        }
        
        
        
    }
}