using System.Collections.Generic;

namespace DTO
{
    public interface IReportService
    {
        public void ChangeReport(Employee employee, string message);

        public void FinishReport(Employee employee);

        public List<Report> GetSubordinateReports(Employee employee);
    }
}