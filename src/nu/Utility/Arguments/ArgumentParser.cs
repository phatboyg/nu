using System.Collections.Generic;

namespace nu.Utility
{
    public class ArgumentParser : IArgumentParser
    {
        public IList<IArgument> Parse(string[] args)
        {
            List<IArgument> arguments = new List<IArgument>();

            foreach (string arg in args)
            {
                arguments.Add(new Argument(arg));
            }

            return arguments;
        }
    }
}