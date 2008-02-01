namespace nu
{
    using System.Collections.Generic;
    using Castle.MicroKernel;
    using Commands;
    using Utility;

    public class Dispatcher
    {
        public const string DEFAULT_COMMAND = "help";

        private readonly IArgumentMapFactory _argumentMapFactory;
        private readonly IArgumentParser _argumentParser;
        private readonly IConsoleHelper _consoleHelper;

        private string _commandName = "help";

        public Dispatcher(IArgumentParser argumentParser, IArgumentMapFactory argumentMapFactory,
                          IConsoleHelper consoleHelper)
        {
            _argumentParser = argumentParser;
            _consoleHelper = consoleHelper;
            _argumentMapFactory = argumentMapFactory;
        }

        [DefaultArgument(DefaultValue = DEFAULT_COMMAND)]
        public string CommandName
        {
            get { return _commandName; }
            set { _commandName = value; }
        }

        public void Forward(string[] args)
        {
            IList<IArgument> argumentList = _argumentParser.Parse(args);
            IArgumentMap dispatcherMap = _argumentMapFactory.CreateMap(this);
            IEnumerable<IArgument> remainingArgs = dispatcherMap.ApplyTo(this, argumentList);

            DefaultToTheHelpCommandIfCommandNameIsNotFound();

            ICommand command = IoC.Resolve<ICommand>(_commandName);
            IArgumentMap commandMap = _argumentMapFactory.CreateMap(command);
            remainingArgs = commandMap.ApplyTo(command, remainingArgs);

            command.Execute(remainingArgs);
        }

        private void DefaultToTheHelpCommandIfCommandNameIsNotFound()
        {
            try
            {
                IoC.Resolve<ICommand>(_commandName);
            }
            catch (ComponentNotFoundException)
            {
                string invalidCommandName = _commandName;
                _commandName = DEFAULT_COMMAND;
                _consoleHelper.WriteError(string.Format("command '{0}' not found", invalidCommandName));
            }
        }
    }
}