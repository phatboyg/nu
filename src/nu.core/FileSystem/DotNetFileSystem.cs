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
namespace nu.core.FileSystem
{
	using System;
	using System.IO;
	using System.Reflection;
	using Configuration;
	using Magnum.Logging;
	using NDepend.Helpers.FileDirectoryPath;

	public class DotNetFileSystem :
		FileSystem
	{
	    ILogger _logger = Logger.GetLogger<DotNetFileSystem>();
		readonly NuConventions _conventions;
		readonly IPath _path;

		public DotNetFileSystem(IPath path, NuConventions conventions)
		{
			_path = path;
			_conventions = conventions;
		}

		public bool FileExists(string filePath)
		{
			return File.Exists(filePath);
		}

		public bool DirectoryExists(string directory)
		{
			return System.IO.Directory.Exists(directory);
		}

		public void Read(string filePath, Action<Stream> action)
		{
			using (var stream = new FileStream(filePath, FileMode.Open))
			{
				action(stream);
			}
		}

		public DirectoryPathAbsolute WorkingDirectory
		{
			get { return new DirectoryPathAbsolute(System.IO.Directory.GetCurrentDirectory()); }
		}

		public DirectoryPathAbsolute InstallDirectory
		{
			get { return new FilePathAbsolute(Assembly.GetEntryAssembly().Location).ParentDirectoryPath; }
		}

		public DirectoryPathAbsolute ProjectRoot
		{
			get
			{
				DirectoryPathAbsolute a = WalkThePathLookingForNu(WorkingDirectory);
				return a.ParentDirectoryPath;
			}
		}

		public DirectoryPathAbsolute ProjectNuDirectory
		{
			get { return ProjectRoot.GetChildDirectoryWithName(_conventions.ProjectDirectoryName); }
		}

		public DirectoryPath ExtensionsDirectory
		{
			get { return InstallDirectory.GetChildDirectoryWithName(_conventions.ExtensionsDirectoryName); }
		}

		public DirectoryPathAbsolute NugsDirectory
		{
			get { return InstallDirectory.GetChildDirectoryWithName(_conventions.NugsDirectoryName); }
		}

		public FilePath ProjectConfig
		{
			get { return ProjectNuDirectory.GetChildFileWithName(_conventions.ConfigurationFileName); }
		}

		public FilePath GlobalConfig
		{
			get { return InstallDirectory.GetChildFileWithName(_conventions.ConfigurationFileName); }
		}

		public void WorkWithTempDir(Action<DirectoryPathAbsolute> tempAction)
		{
			string tempDir = Path.Combine(Path.GetTempPath(), "nu");
			tempDir = Path.Combine(tempDir, Guid.NewGuid().ToString());
			var d = new DirectoryPathAbsolute(tempDir);
			if (!d.Exists)
			{
				d.Create();
			}
			try
			{
				tempAction(d);
			}
			finally
			{
				d.Delete();
			}
		}

		public string GetTempFileName()
		{
			return Path.GetTempFileName();
		}

		public String ReadToEnd(string filePath)
		{
			string contents = "";
			Read(filePath, s =>
				{
					using (var reader = new StreamReader(s))
					{
						contents = reader.ReadToEnd();
					}
				});

			return contents;
		}

		public void Write(string filePath, String contents)
		{
            if(File.Exists(filePath)) File.Delete(filePath);

			using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				using (var writer = new StreamWriter(fs))
				{
					writer.Write(contents);
					writer.Flush();
				}
			}
		}

		public void Write(string filePath, Stream file)
		{
			using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				using (var writer = new StreamWriter(fs))
				{
					writer.Flush();
				}
			}
		}

		public void CreateDirectory(string directoryPath)
		{
			System.IO.Directory.CreateDirectory(directoryPath);
		}

		public void CreateHiddenDirectory(string directoryPath)
		{
			DirectoryInfo di = System.IO.Directory.CreateDirectory(directoryPath);
			di.Attributes |= FileAttributes.Hidden;
		}

		public void Copy(string source, string destination)
		{
			File.Copy(source, destination);
		}

		public bool IsRooted(string path)
		{
			return Path.IsPathRooted(path);
		}

		public string Combine(string firstPath, string secondPath)
		{
			return _path.Combine(firstPath, secondPath);
		}

		public char DirectorySeparatorChar
		{
			get { return _path.DirectorySeparatorChar; }
		}

		public string[] GetDirectories(string path)
		{
			return System.IO.Directory.GetDirectories(path);
		}

		public DirectoryPathAbsolute GetDirectory(string path)
		{
			if (!Path.IsPathRooted(path))
				path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), path);

			return new DirectoryPathAbsolute(path);
		}

		public void CreateProjectAt(string path)
		{
			DirectoryPathAbsolute dir = GetDirectory(path);
			//this needs to be in a biz object
			DirectoryPathAbsolute nu = dir.GetChildDirectoryWithName(_conventions.ProjectDirectoryName);
			nu.Create();
            _logger.Debug(x=>x.Write("Creating .nu directory at '{0}'", nu.Path));
			nu.GetChildFileWithName("nu.conf").Create();
            _logger.Debug(x=>x.Write("Crating nu.conf at '{0}'", nu.Path));
		}

		public Directory GetCurrentDirectory()
		{
			var directory = DirectoryName.GetDirectoryName(System.IO.Directory.GetCurrentDirectory());

			return new DotNetDirectory(directory);
		}

		public DirectoryPathAbsolute WalkThePathLookingForNu(DirectoryPathAbsolute path)
		{
			DirectoryPathAbsolute result = null;

			if (!path.IsRoot())
			{
				DirectoryPathAbsolute bro = path.GetChildDirectoryWithName(_conventions.ProjectDirectoryName);
				if (bro.Exists)
				{
					return bro;
				}
				if (path.HasParentDir)
				{
					result = WalkThePathLookingForNu(path.ParentDirectoryPath);
				}
			}


			return result;
		}

	    public void DeleteFile(string fileName)
	    {
	        if(File.Exists(fileName))
                File.Delete(fileName);
	    }
	}
}