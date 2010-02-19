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
namespace nu
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using core;
	using core.Commands;
	using Magnum;
	using Magnum.CommandLineParser;
	using Magnum.Logging;
	using Magnum.Monads.Parser;
	using StructureMap;
	using util;

	internal class Program
	{
		static readonly ILogger _log = new LoggerBootstrapper().Bootstrap(typeof(Program).Namespace);

		static void Main()
		{
			try
			{
				using (IContainer container = new ContainerBootstrapper().Bootstrap())
				{
					Start(container, init =>
						{
							InitializeDebugCommand(init);

							InitializeExtensions(init, container);
						});
				}
			}
			catch (Exception ex)
			{
				if (_log != null)
					_log.Fatal(ex, "An unhandled exception occurred");
				else
				{
					Console.WriteLine(ex);
				}
			}
		}

		static void Start(IContainer container, Action<ExtensionInitializer> initializeAction)
		{
			_log.Debug("Parsing command line");

			IEnumerable<Command> commands = CommandLine.Parse<Command>(init =>
				{
					var containerConfigurator = new StructureMapContainerConfigurator(container);
					var containerAccess = new StructureMapContainer(container);

					container.Configure(x =>
						{
							x.For<core.Container>().Singleton().Use(containerAccess);
							x.For<ContainerConfigurator>().Singleton().Use(containerConfigurator);
						});

					var initializer = container.With(init).GetInstance<StructureMapExtensionInitializer>();

					initializeAction(initializer);
                }).ToArray();

			if (commands.Any())
			{
				ExecuteCommands(commands);
			}
			else
			{
				_log.Warn("No commands specified");
			}
		}

		static void InitializeDebugCommand(ExtensionInitializer initializer)
		{
			initializer.Add(from debug in initializer.Switch("debug")
			                select initializer.GetCommand<DebugCommand>(new {enabled = debug.Value}));
		}

		static void ExecuteCommands(IEnumerable<Command> commands)
		{
			int count = 0;
			commands.Each(command =>
				{
					command.Execute();
					count++;
				});

			_log.Debug(x => x.Write("{0} command{1} executed", count, (count > 0 ? "s" : "")));
		}

		static void InitializeExtensions(ExtensionInitializer init, IContainer container)
		{
			container.GetAllInstances<Extension>()
				.Each(extension => extension.Initialize(init));
		}
	}
}