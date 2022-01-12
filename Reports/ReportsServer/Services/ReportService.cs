using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO.Exceptions;
using Newtonsoft.Json;

namespace DTO
{
    public class ReportService
    {
        public ReportService(EmployeeService employeeService, TaskService taskService)
        {
            Reports = new List<Report>();
            _employeeService = employeeService;
            _taskService = taskService;
        }
        
        private string ReportsPath { get; } =@"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Reports.json";
        private string WeeklyReportsPath { get; } =@"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\WeeklyReports.json";
        private EmployeeService _employeeService;
        private TaskService _taskService;

        private List<Report> Reports { get; set; }
        private List<WeeklyReport> WeeklyReports { get; set; }
        
        private void SerializeReports()
        {
            string json = JsonConvert.SerializeObject(Reports);
            using var streamWriter = new StreamWriter(ReportsPath);
            streamWriter.WriteLine(json);
        }

        private List<Report> DeserializeReports()
        {
            using var streamReader = new StreamReader(ReportsPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Report>>(json);
        }
        
        private void SerializeWeeklyReports()
        {
            string json = JsonConvert.SerializeObject(WeeklyReports);
            using var streamWriter = new StreamWriter(WeeklyReportsPath);
            streamWriter.WriteLine(json);
        }

        private List<WeeklyReport> DeserializeWeeklyReports()
        {
            using var streamReader = new StreamReader(WeeklyReportsPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<WeeklyReport>>(json);
        }

        public Report AddNewReport(Guid employeeId)
        {
            Reports = DeserializeReports();
            var report = new Report(_employeeService.GetEmployeeById(employeeId));
            Reports.Add(report);
            SerializeReports();
            return report;
        }

        public List<Report> GetAllReports()
        {
            Reports = DeserializeReports();
            return Reports;
        }

        public Report ChangeReportContent(Guid reportId, string message)
        {
            Reports = DeserializeReports();
            Report report = Reports.FirstOrDefault(rep => rep.Id.Equals(reportId));
            if (report == null) throw new ReportNotFoundException();
            report.ChangeReportContent(message);
            SerializeReports();
            return report;
        }

        public Report ChangeReportStatus(Guid reportId, ReportStatusType status)
        {
            Reports = DeserializeReports();
            Report report = Reports.FirstOrDefault(rep => rep.Id.Equals(reportId));
            if (report == null) throw new ReportNotFoundException();
            report.ChangeStatus(status);
            SerializeReports();
            return report;
        }

        public Report FinishReport(Guid reportId)
        {
            Reports = DeserializeReports();
            Report report = Reports.FirstOrDefault(rep => rep.Id.Equals(reportId));
            if (report == null) throw new ReportNotFoundException();
            report.ChangeStatus(ReportStatusType.Finished);
            SerializeReports();
            return report;
        }

        public List<Report> GetSubordinateReports(Guid employeeId)
        {
            Reports = DeserializeReports();
            return Reports.Where(report => report.Employee.LeaderId.Equals(employeeId)).ToList();
        }

        public Report AddTaskToReport(Guid reportId, Guid taskId)
        {
            Reports = DeserializeReports();
            Report report = Reports.FirstOrDefault(rep => rep.Id.Equals(reportId));
            if (report == null) throw new ReportNotFoundException();
            report.AddTask(_taskService.FindTaskById(taskId));
            SerializeReports();
            return report;
        }

        public WeeklyReport MakeWeeklyReport(Guid employeeId)
        {
            DateTime dateTime = DateTime.Now.AddDays(-7);
            Reports = DeserializeReports();
            WeeklyReports = DeserializeWeeklyReports();
            var weeklyReport = new WeeklyReport(_employeeService.GetEmployeeById(employeeId));
            weeklyReport.AddDailyReports(Reports.Where(report => report.CreationTime > dateTime)
                .ToList());
            foreach (Report report in weeklyReport.GetDailyReports())
            {
                weeklyReport.AddTasks(report.Tasks);
            }
            foreach (Report report in weeklyReport.GetDailyReports())
            {
                weeklyReport.AddEmployees(report.Employee);
            }
            WeeklyReports.Add(weeklyReport);
            SerializeReports();
            SerializeWeeklyReports();
            return weeklyReport;
        }

        public List<WeeklyReport> GetWeeklyReports()
        {
            List<WeeklyReport> weeklyReports = DeserializeWeeklyReports();
            return weeklyReports;
        }

        public List<Task> GetWeeklyTasks(Guid weeklyReportId)
        {
            WeeklyReports = DeserializeWeeklyReports();
            WeeklyReport weeklyReport = WeeklyReports.FirstOrDefault(report => report.Id.Equals(weeklyReportId));
            if (weeklyReport == null) throw new ReportNotFoundException();
            return weeklyReport.GetTasks();
        }

        public List<Report> GetDailyReports(Guid weeklyReportId)
        {
            WeeklyReports = DeserializeWeeklyReports();
            WeeklyReport weeklyReport = WeeklyReports.FirstOrDefault(report => report.Id.Equals(weeklyReportId));
            if (weeklyReport == null) throw new ReportNotFoundException();
            return weeklyReport.GetDailyReports();
        }
        
        public List<Employee> GetEmployeesWithReports(Guid weeklyReportId)
        {
            WeeklyReports = DeserializeWeeklyReports();
            WeeklyReport weeklyReport = WeeklyReports.FirstOrDefault(report => report.Id.Equals(weeklyReportId));
            if (weeklyReport == null) throw new ReportNotFoundException();
            return weeklyReport.GetEmployees();
        }
        
        public IEnumerable<Employee> GetEmployeesWithoutReports(Guid weeklyReportId)
        {
            WeeklyReports = DeserializeWeeklyReports();
            WeeklyReport weeklyReport = WeeklyReports.FirstOrDefault(report => report.Id.Equals(weeklyReportId));
            if (weeklyReport == null) throw new ReportNotFoundException();
            return _employeeService.GetAllEmployees().Except(weeklyReport.GetEmployees());
        }
    }
}