using System;

namespace DTO.Exceptions
{
    public class ReportNotFoundException : Exception
    {
        public ReportNotFoundException(string message = "Report not found, please provide correct ID")
            : base(message)
        {
        }
    }
}