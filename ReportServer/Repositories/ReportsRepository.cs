using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO;
using Newtonsoft.Json;

namespace ReportsServer.Repositories
{
    public class ReportsRepository
    {
        public ReportsRepository(string reportsPath, string weeklyReportsPath)
        {
            ReportsPath = reportsPath;
            WeeklyReportsPath = weeklyReportsPath;
            Reports = new List<Report>();
            WeeklyReports = new List<WeeklyReport>();
        }

        private string ReportsPath { get; }
        private string WeeklyReportsPath { get; }
        private List<Report> Reports { get; set; }
        private List<WeeklyReport> WeeklyReports { get; set; }

        public void SerializeReports()
        {
            string json = JsonConvert.SerializeObject(Reports);
            using var streamWriter = new StreamWriter(ReportsPath);
            streamWriter.WriteLine(json);
        }

        public void SerializeWeeklyReports()
        {
            string json = JsonConvert.SerializeObject(WeeklyReports);
            using var streamWriter = new StreamWriter(WeeklyReportsPath);
            streamWriter.WriteLine(json);
        }

        private List<Report> DeserializeReports()
        {
            using var streamReader = new StreamReader(ReportsPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Report>>(json);
        }

        private List<WeeklyReport> DeserializeWeeklyReports()
        {
            using var streamReader = new StreamReader(WeeklyReportsPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<WeeklyReport>>(json);
        }

        public List<Report> GetAllReports()
        {
            return DeserializeReports();
        }

        public List<WeeklyReport> GetAllWeeklyReports()
        {
            return DeserializeWeeklyReports();
        }

        public void AddNewReport(Report report)
        {
            Reports = DeserializeReports();
            Reports.Add(report);
            SerializeReports();
        }

        public void AddNewWeeklyReport(WeeklyReport weeklyReport)
        {
            WeeklyReports = DeserializeWeeklyReports();
            WeeklyReports.Add(weeklyReport);
            SerializeWeeklyReports();
        }

        public Report GetReportById(Guid reportId)
        {
            return GetAllReports().FirstOrDefault(rep => rep.Id.Equals(reportId));
        }

        public WeeklyReport GetWeeklyReportById(Guid weeklyReportId)
        {
            return GetAllWeeklyReports().FirstOrDefault(report => report.Id.Equals(weeklyReportId));
        }
    }
}