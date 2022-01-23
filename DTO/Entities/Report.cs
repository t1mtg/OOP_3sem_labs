using System;
using System.Collections.Generic;

namespace DTO
{
    public enum ReportStatusType
    {
        Draft,
        Unfinished,
        Finished,
    }
    
    public class Report
    {
        
        public Report()
        {
        }
        public Report(Employee employee)
        {
            Employee = employee;
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
            Tasks = new List<Task>();
        }

        public string ReportContent { get; set; }
        public List<Task> Tasks { get; set; }
        public Guid Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime CreationTime { get; set; }
        
        public ReportStatusType ReportStatus { get; set; }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public void ChangeStatus(ReportStatusType newStatus)
        {
            ReportStatus = newStatus;
        }

        public void ChangeReportContent(string message)
        {
            ReportContent = message;
        }
    }
}