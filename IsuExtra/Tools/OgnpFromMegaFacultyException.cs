using System;

namespace IsuExtra.Tools
{
    public class OgnpFromMegaFacultyException : Exception
    {
        public OgnpFromMegaFacultyException(string message = "Ognp from student mega faculty")
            : base(message)
        {
        }
    }
}