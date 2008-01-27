namespace nu.Utility
{
    public class Argument : IArgument
    {
        private readonly string _arg;

        public Argument(string arg)
        {
            _arg = arg;
        }

        public string Value
        {
            get { return _arg; }
        }
    }
}