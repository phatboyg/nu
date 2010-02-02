namespace nu
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           Locator.Resolve<Dispatcher>().Forward(args);
        }
    }
}