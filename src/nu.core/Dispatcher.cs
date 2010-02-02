using System;
using System.Globalization;
using System.Text;
using nu.Resources;

namespace nu
{
    using System.Collections.Generic;
    using Castle.MicroKernel;
    using core.Commands;
    using Model.ArgumentParsing;
    using Model.ArgumentParsing.Exceptions;
    using Utility;

    public class Dispatcher
    {
        public const string DEFAULT_COMMAND = "help";

        private readonly IArgumentMapFactory _argumentMapFactory;
        private readonly IArgumentParser _argumentParser;
        private readonly IConsole _console;

        private string _commandName = nuresources.Dispatcher_DefaultCommand;

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

                IOldCommand oldCommand = WLocator.Resolve<IOldCommand>(_commandName);
                IArgumentMap commandMap = _argumentMapFactory.CreateMap(oldCommand);
                remainingArgs = commandMap.ApplyTo(oldCommand, remainingArgs);
                oldCommand.Execute(remainingArgs);
            }
            catch(MissingRequiredArgumentsException ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Format(CultureInfo.CurrentUICulture,
                    nuresources.Dispatcher_MissingRequiredArguments, Environment.NewLine));

                foreach (ArgumentTarget argumentTarget in ex.ArgumentsMissing)
                {
                    builder.AppendFormat("{1,-20}{2}{0}", Environment.NewLine, "<" + argumentTarget.Property.Name + ">",
                                        argumentTarget.Attribute.Description);
                }
                _console.WriteLine(builder.ToString());
            }

        }

        private void DefaultToTheHelpCommandIfCommandNameIsNotFound()
        {
            try
            {
                WLocator.Resolve<IOldCommand>(_commandName);
            }
            catch (ComponentNotFoundException)
            {
                string invalidCommandName = _commandName;
                _commandName = DEFAULT_COMMAND;
                _console.WriteError(string.Format(CultureInfo.CurrentUICulture, 
                    nuresources.Common_CommandNotFound, invalidCommandName));
            }
        }
    }
}