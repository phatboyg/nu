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
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Model.ArgumentParsing;
	using Model.Package;
	using nu.Model.Package;
	using SubSystems.FileSystem;
	using Utility;

	[Command(Description = "Installs a package into a NU solution.")]
	public class InstallCommand : IOldCommand
	{
		readonly IConsole _console;
		readonly IFileSystem _fileSystem;
		readonly IPackageRepository _packageRepository;

		public InstallCommand(IPackageRepository packageRepository, IFileSystem fileSystem, IConsole console)
		{
			_packageRepository = packageRepository;
			_console = console;
			_fileSystem = fileSystem;
		}

		[Argument(Required = true)]
		public string Package { get; set; }

		public void Execute(IEnumerable<IArgument> arguments)
		{
			if (string.IsNullOrEmpty(Package))
				throw new ArgumentNullException("Product", "You must specify a package to install");

			_console.WriteLine("Injecting {0}", Package);

			Package pkg = _packageRepository.FindByName(Package);

			foreach (PackageItem item in pkg.Items)
			{
				WriteToProject(item);
			}

			_console.WriteLine("Finished Injecting {0}", Package);
		}

		void WriteToProject(PackageItem item)
		{
			string target = item.Target; //lib, tools, src, etc
			string transformedTargetDir = "_someTransformationThing.Get(target)";

			_fileSystem.Copy(item.StorageLocation, Path.Combine(transformedTargetDir, item.FileName));
		}
	}
}