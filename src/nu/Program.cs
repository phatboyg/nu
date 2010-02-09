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
	using StructureMap;
	using util;

	internal class Program
	{
		static void Main()
		{
			ILogger log = null;

			try
			{
				IContainer container = Bootstrapper.Bootstrap();

				log = Logger.GetLogger(typeof(Program).Namespace);

				IEnumerable<Command> commands = CommandLine.Parse<Command>(init =>
					{
						var initializer = new StructureMapExtensionInitializer(init, container);

						InitializeExtensions(initializer, container);
					})
					.ToArray();

				if (commands.Any())
				{
					int count = 0;
					commands.Each(command =>
						{
							command.Execute();
							count++;
						});

					log.Debug(x => x.Write("{0} command{1} executed", count, (count > 0 ? "s" : "")));
				}
				else
				{
					log.Warn("0 commands executed (none specified)");
				}
			}
			catch (Exception ex)
			{
				if (log != null)
					log.Fatal(ex, "An unhandled exception occurred");
				else
				{
					Console.WriteLine(ex);
				}
			}
		}

		static void InitializeExtensions(ExtensionInitializer init, IContainer container)
		{
			container.GetAllInstances<Extension>()
				.Each(extension => extension.Initialize(init));
		}
	}
}