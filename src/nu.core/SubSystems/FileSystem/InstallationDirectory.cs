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
namespace nu.core.SubSystems.FileSystem
{
	using System.Collections.Generic;
	using System.IO;
	using Model.Files;
	using NDepend.Helpers.FileDirectoryPath;
	using Serialization;

    /// <summary>
	/// Where the 'nu.exe' is located
	/// </summary>
	public class InstallationDirectory
	{
		readonly DirectoryPathAbsolute _installationPath;
		readonly DirectoryPathAbsolute _installedNugPath;

		public InstallationDirectory(DirectoryPathAbsolute installationPath)
		{
			_installationPath = installationPath;
			_installedNugPath = _installationPath.GetChildDirectoryWithName("nugs");
		}

		public object GetRegistry()
		{
			string rawJson = GetRegistryFile();
		    return null; // JsonUtil.Get<NuRegistry>(rawJson);
		}

		public string GetRegistryFile()
		{
			//var fi = _installationPath.GetChildFileWithName(NuRegistry.FileName);
			return File.ReadAllText("fi.Path");
		}

		public string[] InstalledNugs()
		{
			var result = new List<string>();
			var bob = _installedNugPath.ChildrenDirectoriesPath;
			foreach (DirectoryPathAbsolute absolute in bob)
			{
				result.Add(absolute.DirectoryName);
			}
			return result.ToArray();
		}

		public bool IsNugAlreadyLocal(string name)
		{
			return _installedNugPath.GetChildDirectoryWithName(name).Exists;
		}

		public void StoreNug(object spec, Stream stream)
		{
			var nugDirectory = _installedNugPath.GetChildDirectoryWithName("spec.Name");
			if (!nugDirectory.Exists)
				Directory.CreateDirectory(nugDirectory.Path);

			var tempName = string.Format("{0}.zip", "spec.Name");
			var tempFile = _installedNugPath.GetChildFileWithName(tempName);
			stream.WriteToDisk(tempFile.Path);
			Zip.Unzip(tempFile, nugDirectory);
			File.Delete(tempFile.Path);
		}

		public object GetNug(string name)
		{
			//take from here
            return null; //new LocalNugInfo();
		}
	}
}