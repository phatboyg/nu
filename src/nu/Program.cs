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
		static ILogger _log;

		static void Main()
		{
			IContainer container = Bootstrapper.Bootstrap();

			_log = Logger.GetLogger(typeof(Program).Namespace);

			try
			{
				IEnumerable<ICommand> commands = CommandLine.Parse<ICommand>(init =>
					{
						var initializer = new StructureMapExtensionInitializer(init, container);

						InitializeExtensions(initializer, container);
					})
					.ToArray();

				if (!commands.Any())
					Console.WriteLine("No command line arguments were valid");

				commands.Each(command => command.Execute());
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception occurred parsing the command line: " + ex);
			}
		}

		static void InitializeExtensions(ExtensionInitializer init, IContainer container)
		{
			IList<Extension> extensions = container.GetAllInstances<Extension>();
			extensions.Each(extension => { extension.Initialize(init); });

//			init.Add(from version in init.Argument("version") select (ICommand)new VersionCommand());
		}
	}
}