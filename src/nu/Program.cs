namespace nu
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           IDispatcher dispatcher = IoC.Resolve<IDispatcher>();
           dispatcher.Forward(args);
        }
    }
}