namespace nu.Model.ArgumentParsing
{
    using System.Collections.Generic;

    public interface IArgumentParser
    {
        IList<IArgument> Parse(string[] args);
    }
}