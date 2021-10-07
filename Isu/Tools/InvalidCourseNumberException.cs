namespace Isu.Tools
{
    public class InvalidCourseNumberException : IsuException
    {
        public InvalidCourseNumberException()
            : base("Invalid course number")
        {
        }
    }
}