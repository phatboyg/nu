namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using Castle.MicroKernel;
    using Utility;

    /// <summary>
    /// Lists all currently registered commands if a name is supplied and their usage.
    /// </summary>
    public class HelpCommand : Command
    {
        private string _commandName;

        [DefaultArgument]
        public string CommandName
        {
            get { return _commandName; }
            set { _commandName = value; }
        }

        public override string Name
        {
            get { return "Help"; }
        }


        public override void Execute()
        {
            IHandler[] handlers = IoC.Container.Kernel.GetAssignableHandlers(typeof (ICommand));

            if (string.IsNullOrEmpty(_commandName))
            {
                DisplayCommandList(handlers);
            }
            else
            {
                DisplayCommandHelp(handlers);
            }
        }

        private void DisplayCommandHelp(IEnumerable<IHandler> handlers)
        {
            foreach (IHandler handler in handlers)
            {
                ICommand cmd = (ICommand) handler.Resolve(CreationContext.Empty);
                string commandName = cmd.Name;
                if (!String.IsNullOrEmpty(commandName) && string.Compare(commandName, _commandName, true) == 0)
                {
                    Console.WriteLine("{0} command:", commandName);
                    // TODO Console.WriteLine(Parser.ArgumentsUsage(cmd.GetType()));

                    return;
                }
            }

            Console.WriteLine("Command not found: {0}", _commandName);
        }

        private static void DisplayCommandList(IEnumerable<IHandler> handlers)
        {
            Console.WriteLine("Commands currently registered:" + Environment.NewLine);

            foreach (IHandler handler in handlers)
            {
                ICommand cmd = (ICommand) handler.Resolve(CreationContext.Empty);
                string commandName = cmd.Name;
                if (!String.IsNullOrEmpty(commandName))
                {
                    Console.WriteLine("Command Help: {0}:", commandName);
                    // TODO Console.WriteLine(Parser.ArgumentsUsage(cmd.GetType()));
                }
            }
        }
    }
}