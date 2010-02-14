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
	using FileSystem;
	using Magnum.Logging;

	public class FileBasedProjectConfiguration :
		FileBasedConfiguration,
		ProjectConfiguration
	{
		readonly NuConventions _conventions;
		readonly GlobalConfiguration _globalConfiguration;
		static readonly ILogger _logger = Logger.GetLogger<FileBasedProjectConfiguration>();

		public FileBasedProjectConfiguration(FileSystem fileSystem, GlobalConfiguration globalConfiguration, NuConventions conventions)
			: base(fileSystem, GetFile(fileSystem, conventions))
		{
			_globalConfiguration = globalConfiguration;
			_conventions = conventions;

			OnMissing = GetGlobalConfigurationValue;
		}

		public Directory ProjectRoot
		{
			get
			{
				Directory a = WalkThePathLookingForNu(FileSystem.GetCurrentDirectory(), _conventions);

				_logger.Debug(x => x.Write("Project Root: {0}", a.Name));
				return a.Parent;
			}
		}

		public Directory ProjectNuDirectory
		{
			get { return ProjectRoot.GetChildDirectory(_conventions.ProjectDirectoryName); }
		}

		string GetGlobalConfigurationValue(string key)
		{
			_logger.Debug(x => x.Write("Falling back to global config for key '{0}'", key));
			return _globalConfiguration[key];
		}

		public static File GetFile(FileSystem fileSystem, NuConventions conventions)
		{
			Directory a = WalkThePathLookingForNu(fileSystem.GetCurrentDirectory(), conventions);

			return a.GetChildFile(conventions.ConfigurationFileName);
		}

		static Directory WalkThePathLookingForNu(Directory direc, NuConventions conventions)
		{
			Directory result = null;

			if (!direc.IsRoot())
			{
				Directory bro = direc.GetChildDirectory(conventions.ProjectDirectoryName);
				if (bro.Exists())
				{
					_logger.Debug(x => x.Write("Found the nu folder: {0}", bro.Name));

					return bro;
				}
				if (direc.HasParentDir)
				{
					result = WalkThePathLookingForNu(direc.Parent, conventions);
				}
			}

			return result;
		}
	}
}