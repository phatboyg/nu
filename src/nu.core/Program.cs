namespace nu
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           WLocator.Resolve<Dispatcher>().Forward(args);
        }
    }
}