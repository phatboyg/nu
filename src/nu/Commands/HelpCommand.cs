namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using Castle.MicroKernel;
    using Utility;

    /// <summary>
    /// Lists all currently registered commands if a name is supplied and their usage.
    /// </summary>
    [Command(Description = "Displays help for the command-line syntax")]
    public class HelpCommand : ICommand
    {
        private readonly IArgumentMapFactory _argumentMapFactory;

        private string _commandName;

        public HelpCommand(IArgumentMapFactory argumentMapFactory)
        {
            _argumentMapFactory = argumentMapFactory;
        }

        [DefaultArgument(Description = "Display detailed help for the specified command")]
        public string CommandName
        {
            get { return _commandName; }
            set { _commandName = value; }
        }

        public void Execute(IEnumerator<IArgument> arguments)
        {
            IHandler[] handlers = IoC.Container.Kernel.GetAssignableHandlers(typeof (ICommand));

            if (string.IsNullOrEmpty(_commandName))
            {
                DisplayCommandList(handlers);
            }
            else
            {
                DisplayCommandHelp();
            }
        }

        private void DisplayCommandHelp()
        {
            ICommand command = IoC.Resolve<ICommand>(_commandName);

            IArgumentMap map = _argumentMapFactory.CreateMap(command);

            Console.WriteLine("Usage: nu {0} {1}", _commandName, map.Usage);
        }

        private static void DisplayCommandList(IEnumerable<IHandler> handlers)
        {
            Console.WriteLine("Available Commands:" + Environment.NewLine);

            foreach (IHandler handler in handlers)
            {
                ICommand cmd = (ICommand) handler.Resolve(CreationContext.Empty);

                string description = string.Empty;
                object[] attributes = cmd.GetType().GetCustomAttributes(typeof (CommandAttribute), false);
                if (attributes.Length == 1)
                    description = ((CommandAttribute) attributes[0]).Description;

                Console.WriteLine("{0,-20}{1}", handler.ComponentModel.Name, description);
            }
        }
    }
}