namespace Isu
{
    internal class Program
    {
        private static void Main()
        {
            var isuService = new IsuService();
            isuService.AddGroup("M3133");
        }
    }
}
