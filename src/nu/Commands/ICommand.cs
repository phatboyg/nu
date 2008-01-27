namespace nu.Commands
{
    using System.Collections.Generic;
    using Utility;

    public interface ICommand
    {
        string Name { get; }
        void Route(IEnumerator<IArgument> arguments);
        void Execute();
    }
}