using System;
using System.Collections.Generic;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace ReportsServer.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportController : ControllerBase
    {
        private ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [Route("addReport")]
        [HttpPost]
        public Report AddNewReport([FromQuery] Guid employeeId)
        {
            return _reportService.AddNewReport(employeeId);
        }

        [Route("getReports")]
        [HttpGet]
        public List<Report> GetAllReports()
        {
            return _reportService.GetAllReports();
        }

        [Route("changeContent")]
        [HttpPost]
        public Report ChangeReportContent([FromQuery] Guid reportId, [FromQuery] string message)
        {
            return _reportService.ChangeReportContent(reportId, message);
        }
        
        [Route("changeStatus")]
        [HttpPost]
        public Report ChangeReportStatus([FromQuery] Guid reportId, [FromQuery] ReportStatusType status)
        {
            return _reportService.ChangeReportStatus(reportId, status);
        }
        
        [Route("finishReport")]
        [HttpPost]
        public Report FinishReport([FromQuery] Guid reportId)
        {
            return _reportService.FinishReport(reportId);
        }

        [Route("getSubordinateReports")]
        [HttpGet]
        public List<Report> GetSubordinateReports([FromQuery] Guid employeeId)
        {
            return _reportService.GetSubordinateReports(employeeId);
        }

        [Route("addTaskToReport")]
        [HttpPost]
        public Report AddTaskToReport([FromQuery] Guid reportId, [FromQuery] Guid taskId)
        {
            return _reportService.AddTaskToReport(reportId, taskId);
        }

        [Route("makeWeeklyReport")]
        [HttpPost]
        public WeeklyReport MakeWeeklyReport([FromQuery] Guid employeeId)
        {
            return _reportService.MakeWeeklyReport(employeeId);
        }
        
        [Route("getWeeklyReports")]
        [HttpGet]
        public List<WeeklyReport> GetWeeklyReports()
        {
            return _reportService.GetWeeklyReports();
        }

        [Route("getWeeklyTasks")]
        [HttpGet]
        public List<Task> GetWeeklyTasks([FromQuery] Guid weeklyReportId)
        {
            return _reportService.GetWeeklyTasks(weeklyReportId);
        }
        
        [Route("getDailyReports")]
        [HttpGet]
        public List<Report> GetDailyReports([FromQuery] Guid weeklyReportId)
        {
            return _reportService.GetDailyReports(weeklyReportId);
        }
        
        [Route("getEmployeesWithReports")]
        [HttpGet]
        public List<Employee> GetEmployeesWithReports([FromQuery] Guid weeklyReportId)
        {
            return _reportService.GetEmployeesWithReports(weeklyReportId);
        }
        
        [Route("getEmployeesWithoutReports")]
        [HttpGet]
        public IEnumerable<Employee> GetEmployeesWithoutReports([FromQuery] Guid weeklyReportId)
        {
            return _reportService.GetEmployeesWithoutReports(weeklyReportId);
        }
    }
}