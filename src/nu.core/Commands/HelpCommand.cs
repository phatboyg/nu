// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.core.Commands
{
	using System.Collections.Generic;
	using System.Globalization;
	using Castle.MicroKernel;
	using Model.ArgumentParsing;
	using Resources;
	using SubSystems.Locator;
	using Utility;

	/// <summary>
	/// Lists all currently registered commands if a name is supplied and their usage.
	/// </summary>
	[Command(Description = "Displays help for the command-line syntax")]
	public class HelpCommand : IOldCommand
	{
		readonly IArgumentMapFactory _argumentMapFactory;
		readonly IConsole _console;
		string _commandName;

		public HelpCommand(IArgumentMapFactory argumentMapFactory, IConsole console)
		{
			_argumentMapFactory = argumentMapFactory;
			_console = console;
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

		void DisplayCommandHelp()
		{
			IOldCommand oldCommand;
			try
			{
				oldCommand = WLocator.Resolve<IOldCommand>(_commandName);
			}
			catch (ComponentNotFoundException)
			{
				_console.WriteError(string.Format(CultureInfo.CurrentUICulture,
					nuresources.Common_CommandNotFound, _commandName));
				return;
			}

			IArgumentMap map = _argumentMapFactory.CreateMap(oldCommand);
			_console.WriteHeading(string.Format(CultureInfo.CurrentUICulture, nuresources.Help_CommandName, _commandName));
			_console.WriteLine(nuresources.Help_CommandUsage, _commandName, map.Usage);
		}

		void DisplayCommandList()
		{
			IHandler[] handlers = WLocator.Container.Kernel.GetAssignableHandlers(typeof(IOldCommand));
			if (handlers == null)
				return;

			_console.WriteHeading(nuresources.Help_AvailableCommands);

			foreach (IHandler handler in handlers)
			{
				var cmd = (IOldCommand)handler.Resolve(CreationContext.Empty);

				string description = string.Empty;
				object[] attributes = cmd.GetType().GetCustomAttributes(typeof(CommandAttribute), false);
				if (attributes.Length == 1)
					description = ((CommandAttribute)attributes[0]).Description;

				string line = string.Format("{0,-20}{1}", handler.ComponentModel.Name, description);

				_console.WriteLine(line);
			}
		}
	}
}