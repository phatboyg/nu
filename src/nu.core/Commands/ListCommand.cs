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
	using Model.ArgumentParsing;
	using Model.Package;
	using nu.Model.Package;
	using Utility;

	[Command(Description = "List packages available in the local repository.")]
	public class ListCommand : IOldCommand
	{
		readonly IConsole _console;
		readonly IPackageRepository _packageRepository;

		public ListCommand(IPackageRepository packageRepository, IConsole console)
		{
			_packageRepository = packageRepository;
			_console = console;
		}

		public void Execute(IEnumerable<IArgument> arguments)
		{
			IEnumerable<Package> packages = _packageRepository.FindAll();

			if (packages == null)
			{
				_console.WriteLine("No packages installed. Get to it!");
				return;
			}

			foreach (Package package in packages)
			{
				_console.WriteLine(package.Name);
			}
		}
	}
}