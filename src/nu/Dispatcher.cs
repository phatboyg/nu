using System;
using System.Text;
using nu.Utility.Exceptions;

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
        private readonly IConsole _console;

        private string _commandName = "help";

        public Dispatcher(IArgumentParser argumentParser, IArgumentMapFactory argumentMapFactory,
                          IConsole console)
        {
            _argumentParser = argumentParser;
            _console = console;
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
            try
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
            catch(MissingRequiredArgumentsException ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append( Environment.NewLine + "The following arguments are required but were not provided:"
                    + Environment.NewLine);

                foreach (ArgumentTarget argumentTarget in ex.ArgumentsMissing)
                {
                    builder.AppendFormat("{0,-20}{1}" + Environment.NewLine, "<" + argumentTarget.Property.Name + ">",
                                        argumentTarget.Attribute.Description);
                }
                _console.WriteLine(builder.ToString());
            }

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
                _console.WriteError(string.Format("command '{0}' not found", invalidCommandName));
            }
        }
    }
}