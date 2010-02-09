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
namespace nu.core.Configuration
{
	using System.Collections.Generic;
	using System.Reflection;
	using FileSystem;

	public class GlobalFileBasedConfiguration :
		FileBasedConfiguration,
		GlobalConfiguration
	{
		readonly NuConventions _conventions;

		public GlobalFileBasedConfiguration(IFileSystem fileSystem, NuConventions conventions, IEnumerable<Extension> extensions)
			: base(fileSystem, fileSystem.GlobalConfig)
		{
			Extensions = extensions;

			Defaults = new DefaultConfiguration();

			OnMissing = GetGlobalConfigurationValue;
			_conventions = conventions;
		}

		Configuration Defaults { get; set; }

		public IEnumerable<Extension> Extensions { get; private set; }

		public Directory WorkingDirectory
		{
			get { return FileSystem.GetCurrentDirectory(); }
		}

		public Directory NuInstallDirectory
		{
			get { return new DotNetDirectory(DirectoryName.GetDirectoryNameFromFileName(Assembly.GetEntryAssembly().Location)); }
		}

		public Directory ExtensionsDirectory
		{
			get { return NuInstallDirectory.GetChildDirectory(_conventions.ExtensionsDirectoryName); }
		}

		public Directory NugsDirectory
		{
			get { return NuInstallDirectory.GetChildDirectory(_conventions.NugsDirectoryName); }
		}

		string GetGlobalConfigurationValue(string key)
		{
			return Defaults[key];
		}
	}
}