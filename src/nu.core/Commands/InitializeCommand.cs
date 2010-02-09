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
	using FileSystem;
	using Magnum.Logging;

	public class InitializeCommand :
		Command
	{
		readonly IFileSystem _fileSystem;
		readonly ILogger _log = Logger.GetLogger<GetGlobalConfigurationCommand>();
		readonly string _path;

		public InitializeCommand(string path, IFileSystem fileSystem)
		{
			_path = path;
			_fileSystem = fileSystem;
		}

		public void Execute()
		{
			if (_fileSystem.DirectoryExists(_path))
			{
				_log.Debug(x => x.Write("Initializing path: {0}", _path));

				_fileSystem.CreateProjectAt(_path);
			}
			else
			{
				_log.Warn(x => x.Write("Directory '{0}' does not exist.", _path));
			}
		}
	}
}