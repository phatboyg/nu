namespace nu
{
   using System;
   using System.Collections.Generic;
   using Commands;
   using Utility;

   public class Dispatcher
   {
      public const string DEFAULT_COMMAND = "help";

      private readonly IArgumentMapFactory _argumentMapFactory;
      private readonly IArgumentParser _argumentParser;
      private readonly IConsoleHelper _consoleHelper;

      private string _commandName = "help";

      public Dispatcher(IArgumentParser argumentParser, IArgumentMapFactory argumentMapFactory, IConsoleHelper consoleHelper)
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
         IEnumerator<IArgument> argumentEnumerator = argumentList.GetEnumerator();
         IArgumentMap dispatcherMap = _argumentMapFactory.CreateMap(this);
         dispatcherMap.ApplyTo(this, argumentEnumerator);
         
         DefaultToTheHelpCommandIfCommandNameIsNotFound();
          
         ICommand command = IoC.Resolve<ICommand>(_commandName);
         IArgumentMap commandMap = _argumentMapFactory.CreateMap(command);
         commandMap.ApplyTo(command, argumentEnumerator); 
         command.Execute(argumentEnumerator);
      }

      private void DefaultToTheHelpCommandIfCommandNameIsNotFound()
      {
         try
         {
            IoC.Resolve<ICommand>(_commandName);
         }
         catch (Castle.MicroKernel.ComponentNotFoundException)
         {
            string invalidCommandName = _commandName;
            _commandName = DEFAULT_COMMAND;
            _consoleHelper.WriteError(string.Format("command '{0}' not found", invalidCommandName));
         }
      }
   }
}