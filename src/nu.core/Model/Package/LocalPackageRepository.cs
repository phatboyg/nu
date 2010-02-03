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
namespace nu.core.Model.Package
{
	using System.Collections.Generic;
	using nu.Model.Package;
	using SubSystems.FileSystem;
	using Utility;

	public class LocalPackageRepository : IPackageRepository
	{
		readonly IFileSystem _fileSystem;
		readonly IPath _path;

		public LocalPackageRepository(IPath path, IFileSystem fileSystem)
		{
			_path = path;
			_fileSystem = fileSystem;
		}

		string PathToPackages
		{
			get { return _path.Combine(_fileSystem.GetNuRoot, "packages"); }
		}

		#region IPackageRepository Members

		public Package FindByName(string package)
		{
			Package result = null;

			foreach (Package p in FindAll())
			{
				if (p.Name.Equals(package))
				{
					result = p;
					break;
				}
			}

			return result;
		}

		public IEnumerable<Package> FindAll()
		{
			var result = new List<Package>();

			foreach (var s in GetPackageNames())
			{
				result.Add(new Package(s[0], s[1]));
			}

			return result;
		}

		#endregion

		List<string[]> GetPackageNames()
		{
			string[] dirs = _fileSystem.GetDirectories(PathToPackages);
			var results = new List<string[]>();

			foreach (string dir in dirs)
			{
				int i = dir.LastIndexOf(_fileSystem.DirectorySeparatorChar);

				results.Add(new[] {dir.Substring(i + 1), dir});
			}

			return results;
		}
	}
}