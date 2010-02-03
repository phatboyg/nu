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
namespace nu.core
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Castle.MicroKernel;
	using Commands;
	using Model.ArgumentParsing;
	using Model.ArgumentParsing.Exceptions;
	using Resources;
	using SubSystems.Locator;
	using Utility;

	public class Dispatcher
	{
		public const string DEFAULT_COMMAND = "help";

		readonly IArgumentMapFactory _argumentMapFactory;
		readonly IArgumentParser _argumentParser;
		readonly IConsole _console;

		string _commandName = nuresources.Dispatcher_DefaultCommand;

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

				var oldCommand = WLocator.Resolve<IOldCommand>(_commandName);
				IArgumentMap commandMap = _argumentMapFactory.CreateMap(oldCommand);
				remainingArgs = commandMap.ApplyTo(oldCommand, remainingArgs);
				oldCommand.Execute(remainingArgs);
			}
			catch (MissingRequiredArgumentsException ex)
			{
				var builder = new StringBuilder();
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

		void DefaultToTheHelpCommandIfCommandNameIsNotFound()
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