namespace nu.Commands
{
    using System.Collections.Generic;
    using Model.ArgumentParsing;

    public interface ICommand
    {
        void Execute(IEnumerable<IArgument> arguments);
    }
}