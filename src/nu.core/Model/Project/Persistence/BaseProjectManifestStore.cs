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
namespace nu.core.Model.Project.Persistence
{
	using System;
	using nu.Model.Project;
	using SubSystems.FileSystem;

	public class BaseProjectManifestStore : IProjectManifestStore
	{
		protected const string PROJECT_MANIFEST_DIRECTORY = ".nu";
		protected const string PROJECT_MANIFEST_FILE = "project.nu";
		readonly IFileSystem _fileSystem;

		public BaseProjectManifestStore(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		public virtual IProjectManifest Load(IProjectEnvironment environment)
		{
			throw new NotImplementedException();
		}

		public virtual void Save(IProjectEnvironment environment, IProjectManifest projectManifest)
		{
			throw new NotImplementedException();
		}

		public virtual bool Exists(IProjectEnvironment environment)
		{
			string manifestPath = GetManifestPath(environment);
			return _fileSystem.Exists(manifestPath);
		}

		public virtual string GetProjectDirectory(IProjectEnvironment environment)
		{
			if (!String.IsNullOrEmpty(environment.ProjectDirectory))
			{
				if (_fileSystem.IsRooted(environment.ProjectDirectory))
				{
					return
						String.IsNullOrEmpty(environment.ProjectName)
							? environment.ProjectDirectory
							: _fileSystem.Combine(environment.ProjectDirectory, environment.ProjectName);
				}
				else
				{
					string path;
					if (environment.IsTemplate)
						path = _fileSystem.Combine(_fileSystem.ExecutingDirectory, environment.ProjectDirectory);
					else
						path = _fileSystem.Combine(_fileSystem.CurrentDirectory, environment.ProjectDirectory);

					if (!String.IsNullOrEmpty(environment.ProjectName))
						path = _fileSystem.Combine(path, environment.ProjectName);

					return path;
				}
			}
			else
				return
					String.IsNullOrEmpty(environment.ProjectName)
						? _fileSystem.CurrentDirectory
						: _fileSystem.Combine(_fileSystem.CurrentDirectory, environment.ProjectName);
		}

		public virtual string GetProjectName(IProjectEnvironment environment)
		{
			if (String.IsNullOrEmpty(environment.ProjectName))
			{
				int startIdx = environment.ProjectDirectory.LastIndexOf(_fileSystem.DirectorySeparatorChar.ToString()) + 1;
				return environment.ProjectDirectory.Substring(startIdx);
			}
			else
			{
				return environment.ProjectName;
			}
		}

		public virtual string GetManifestPath(IProjectEnvironment environment)
		{
			return _fileSystem.Combine(GetProjectDirectory(environment),
				_fileSystem.Combine(PROJECT_MANIFEST_DIRECTORY, PROJECT_MANIFEST_FILE));
		}

		protected virtual void PrepareManifestDirectory(IProjectEnvironment environment)
		{
			if (!Exists(environment))
			{
				string manifestPath = GetManifestPath(environment);
				_fileSystem.CreateHiddenDirectory(
					RemoveManifestFileName(manifestPath));
			}
		}

		string RemoveManifestFileName(string file)
		{
			return file.Substring(0, file.LastIndexOf(_fileSystem.DirectorySeparatorChar));
		}
	}
}