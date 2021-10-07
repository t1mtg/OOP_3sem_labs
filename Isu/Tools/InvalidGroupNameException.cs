namespace Isu.Tools
{
    public class InvalidGroupNameException : IsuException
    {
        public InvalidGroupNameException()
            : base("Invalid group name")
        {
        }
    }
}