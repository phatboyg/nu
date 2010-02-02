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
	using core.Commands;
	using Magnum;
	using Magnum.CommandLineParser;
	using Magnum.Monads.Parser;

	internal class Program
	{
		static void Main()
		{
			try
			{
				IEnumerable<ICommand> commands = CommandLine.Parse<ICommand>(init =>
					{
						init.Add(from version in init.Argument("version") select (ICommand)new VersionCommand());
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
	}
}