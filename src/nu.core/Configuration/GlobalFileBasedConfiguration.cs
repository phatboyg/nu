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
    using System;
    using System.Reflection;
	using FileSystem;
    using Magnum.Logging;

    public class GlobalFileBasedConfiguration :
		FileBasedConfiguration,
		GlobalConfiguration
	{
        readonly ILogger _logger = Logger.GetLogger<GlobalFileBasedConfiguration>();
		readonly NuConventions _conventions;

		public GlobalFileBasedConfiguration(DefaultsConfiguration defaults, FileSystem fileSystem, NuConventions conventions)
			: base(fileSystem, fileSystem.GlobalConfig)
		{
			Defaults = defaults;

			OnMissing = GetDefaultConfigurationValue;
			_conventions = conventions;
		}

		DefaultsConfiguration Defaults { get; set; }

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

		string GetDefaultConfigurationValue(string key)
		{
            _logger.Debug(x=>x.Write("Falling back to DEFAULTS for key '{0}'", key));

			return Defaults[key];
		}
	}
}