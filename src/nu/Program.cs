namespace nu
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           IoC.Resolve<Dispatcher>().Forward(args);
        }
    }
}