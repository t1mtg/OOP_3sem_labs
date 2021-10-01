namespace Isu.Tools
{
    public class StudentDoesNotExistException : IsuException
    {
        public StudentDoesNotExistException()
            : base("This student doesn't exist")
        {
        }
    }
}