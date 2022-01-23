using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO.Exceptions;
using Newtonsoft.Json;
using ReportsServer.Repositories;

namespace DTO
{
    public class ReportService
    {
        public ReportService(EmployeeService employeeService, TaskService taskService, ReportsRepository repository)
        {
            _reportsRepository = repository;
            _employeeService = employeeService;
            _taskService = taskService;
        }
        
        private string ReportsPath { get; } =@"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Reports.json";
        private string WeeklyReportsPath { get; } =@"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\WeeklyReports.json";
        private readonly EmployeeService _employeeService;
        private readonly TaskService _taskService;
        private readonly ReportsRepository _reportsRepository;
        
        public Report AddNewReport(Guid employeeId)
        {
            var report = new Report(_employeeService.GetEmployeeById(employeeId));
            _reportsRepository.AddNewReport(report);
            _reportsRepository.SerializeReports();
            return report;
        }

        public List<Report> GetAllReports()
        {
            return _reportsRepository.GetAllReports();
        }

        public Report ChangeReportContent(Guid reportId, string message)
        {
            Report report = _reportsRepository.GetReportById(reportId);
            if (report == null) throw new ReportNotFoundException();
            report.ChangeReportContent(message);
            _reportsRepository.SerializeReports();
            return report;
        }

        public Report ChangeReportStatus(Guid reportId, ReportStatusType status)
        {
            Report report = _reportsRepository.GetReportById(reportId);
            if (report == null) throw new ReportNotFoundException();
            report.ChangeStatus(status);
            _reportsRepository.SerializeReports();
            return report;
        }

        public Report FinishReport(Guid reportId)
        {
            Report report = _reportsRepository.GetReportById(reportId);
            if (report == null) throw new ReportNotFoundException();
            report.ChangeStatus(ReportStatusType.Finished);
            _reportsRepository.SerializeReports();
            return report;
        }

        public List<Report> GetSubordinateReports(Guid employeeId)
        {
            return _reportsRepository.GetAllReports()
                .Where(report => report.Employee.LeaderId.Equals(employeeId))
                .ToList();
        }

        public Report AddTaskToReport(Guid reportId, Guid taskId)
        {
            Report report = _reportsRepository.GetReportById(reportId);
            if (report == null) throw new ReportNotFoundException();
            report.AddTask(_taskService.FindTaskById(taskId));
            _reportsRepository.SerializeReports();
            return report;
        }

        public WeeklyReport MakeWeeklyReport(Guid employeeId)
        {
            DateTime dateTime = DateTime.Now.AddDays(-7);
            var weeklyReport = new WeeklyReport(_employeeService.GetEmployeeById(employeeId));
            weeklyReport.AddDailyReports(_reportsRepository.GetAllReports()
                .Where(report => report.CreationTime > dateTime)
                .ToList());
            foreach (Report report in weeklyReport.GetDailyReports())
            {
                weeklyReport.AddTasks(report.Tasks);
            }
            foreach (Report report in weeklyReport.GetDailyReports())
            {
                weeklyReport.AddEmployees(report.Employee);
            }
            _reportsRepository.AddNewWeeklyReport(weeklyReport);
            _reportsRepository.SerializeReports();
            _reportsRepository.SerializeWeeklyReports();
            return weeklyReport;
        }

        public List<WeeklyReport> GetWeeklyReports()
        {
            return _reportsRepository.GetAllWeeklyReports();
        }

        public List<Task> GetWeeklyTasks(Guid weeklyReportId)
        {
            WeeklyReport weeklyReport = _reportsRepository.GetWeeklyReportById(weeklyReportId);
            if (weeklyReport == null) throw new ReportNotFoundException();
            return weeklyReport.GetTasks();
        }

        public List<Report> GetDailyReports(Guid weeklyReportId)
        {
            WeeklyReport weeklyReport = _reportsRepository.GetWeeklyReportById(weeklyReportId);
            if (weeklyReport == null) throw new ReportNotFoundException();
            return weeklyReport.GetDailyReports();
        }
        
        public List<Employee> GetEmployeesWithReports(Guid weeklyReportId)
        {
            WeeklyReport weeklyReport = _reportsRepository.GetWeeklyReportById(weeklyReportId);
            if (weeklyReport == null) throw new ReportNotFoundException();
            return weeklyReport.GetEmployees();
        }
        
        public IEnumerable<Employee> GetEmployeesWithoutReports(Guid weeklyReportId)
        {
            WeeklyReport weeklyReport = _reportsRepository.GetWeeklyReportById(weeklyReportId);
            if (weeklyReport == null) throw new ReportNotFoundException();
            return _employeeService.GetAllEmployees().Except(weeklyReport.GetEmployees());
        }
    }
}