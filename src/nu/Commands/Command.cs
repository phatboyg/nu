namespace nu.Commands
{
    using System.Collections.Generic;
    using Utility;

    public abstract class Command : ICommand
    {
        #region ICommand Members

        public virtual void Route(IEnumerator<IArgument> arguments)
        {
            // TODO Parser.ParseArgumentsWithUsage(args, this);

            Execute();
        }

        public abstract void Execute();

        public virtual string Name
        {
            get { return GetType().Name.Replace("Command", ""); }
        }

        #endregion
    }
}