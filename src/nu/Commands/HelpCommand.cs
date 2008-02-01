namespace nu.Commands
{
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
      private readonly IConsoleHelper _consoleHelper;
      private string _commandName;

      public HelpCommand(IArgumentMapFactory argumentMapFactory, IConsoleHelper consoleHelper)
      {
         _argumentMapFactory = argumentMapFactory;
         _consoleHelper = consoleHelper;
      }

      [DefaultArgument(Description = "Display detailed help for the specified command")]
      public string CommandName
      {
         get { return _commandName; }
         set { _commandName = value; }
      }

      public void Execute(IEnumerable<IArgument> arguments)
      {
         if (string.IsNullOrEmpty(_commandName))
         {
            DisplayCommandList();
         }
         else
         {
            DisplayCommandHelp();
         }
      }

      private void DisplayCommandHelp()
      {
         ICommand command;
         try
         {
            command = IoC.Resolve<ICommand>(_commandName);
         }
         catch (ComponentNotFoundException)
         {
            _consoleHelper.WriteError(string.Format("command '{0}' not found", _commandName));
            return;
         }

         IArgumentMap map = _argumentMapFactory.CreateMap(command);
         _consoleHelper.WriteHeading(string.Format("Command: {0}", _commandName));
         _consoleHelper.WriteLine("Usage: nu {0} {1}", _commandName, map.Usage);
      }

      private void DisplayCommandList()
      {
         IHandler[] handlers = IoC.Container.Kernel.GetAssignableHandlers(typeof (ICommand));
         if (handlers == null) return;

         _consoleHelper.WriteHeading("Available Commands");

         foreach (IHandler handler in handlers)
         {
            ICommand cmd = (ICommand) handler.Resolve(CreationContext.Empty);

            string description = string.Empty;
            object[] attributes = cmd.GetType().GetCustomAttributes(typeof (CommandAttribute), false);
            if (attributes.Length == 1)
               description = ((CommandAttribute) attributes[0]).Description;

            string line = string.Format("{0,-20}{1}", handler.ComponentModel.Name, description);

            _consoleHelper.WriteLine(line);
         }
      }
   }
}