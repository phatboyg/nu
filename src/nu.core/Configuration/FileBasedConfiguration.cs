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
	using System.IO;
	using Magnum.Logging;
	using NDepend.Helpers.FileDirectoryPath;
	using SubSystems.FileSystem;
	using SubSystems.Serialization;

	public class FileBasedConfiguration
	{
		readonly IFileSystem _fileSystem;
		readonly ILogger _log = Logger.GetLogger<FileBasedConfiguration>();
		protected Func<string, string> OnMissing = DefaultMissingKeyHandler;

		public FileBasedConfiguration(IFileSystem fileSystem, FilePath configurationPath)
		{
			_fileSystem = fileSystem;

			Entries = ReadExistingConfigurationFromFile(configurationPath);
		}

		public virtual string this[string key]
		{
			get
			{
				Entry entry = Entries.Get(key);
				if (entry != null)
					return entry.Value;

				return OnMissing(key);
			}

			set { Entries.Get(key, x => x.SetValue(value)); }
		}


		public bool Contains(string key)
		{
			return Entries.Contains(key);
		}

		protected Entries Entries { get; private set; }

		Entries ReadExistingConfigurationFromFile(BasePath configurationPath)
		{
			if (!File.Exists(configurationPath.Path))
			{
				_log.Debug(x => x.Write("No existing configuration file found: {0}", configurationPath.Path));

				return new Entries();
			}

			return JsonUtil.Get<Entries>(_fileSystem.ReadToEnd(configurationPath.Path));
		}

		protected static string DefaultMissingKeyHandler(string key)
		{
			throw new ConfigurationException("No configuration entry for '" + key + "' was found");
		}
	}
}