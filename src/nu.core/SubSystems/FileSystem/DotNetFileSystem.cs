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
	using System;
	using System.IO;
	using System.Reflection;
	using NDepend.Helpers.FileDirectoryPath;
	using Utility;

	public class DotNetFileSystem :
        IFileSystem
	{
		readonly IPath _path;

		public DotNetFileSystem(IPath path)
		{
			_path = path;
		}

		public bool Exists(string filePath)
		{
			return File.Exists(filePath);
		}

		public bool DirectoryExists(string directory)
		{
			return Directory.Exists(directory);
		}

		public void Read(string filePath, Action<Stream> action)
		{
            using(var stream = new FileStream(filePath, FileMode.Open))
            {
                action(stream);
            }
		}

	    public DirectoryPath WorkingDirectory
	    {
            get { return new DirectoryPathAbsolute(Directory.GetCurrentDirectory()); }
	    }

	    public DirectoryPathAbsolute InstallDirectory
	    {
            get
            {
                return new FilePathAbsolute(Assembly.GetEntryAssembly().Location).ParentDirectoryPath;
            }
	    }

	    public DirectoryPathAbsolute ProjectRoot
	    {
            get { return new DirectoryPathAbsolute(""); }
	    }

	    public DirectoryPathAbsolute ProjectNuDirectory
	    {
            get { return ProjectRoot.GetChildDirectoryWithName(".nu"); }
	    }

	    public DirectoryPath ExtensionsDirectory
	    {
            get { return InstallDirectory.GetChildDirectoryWithName("extensions"); }
	    }

	    public FilePath ProjectConfig
	    {
            get
            {
                return ProjectNuDirectory.GetChildFileWithName("nu.conf");
            }
	    }

	    public FilePath GlobalConfig
	    {
            get { return InstallDirectory.GetChildFileWithName("nu.conf"); }
	    }

	    public void WorkWithTempDir(Action<DirectoryPath> tempAction)
	    {
	        throw new NotImplementedException();
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
			Directory.CreateDirectory(directoryPath);
		}

		public void CreateHiddenDirectory(string directoryPath)
		{
			DirectoryInfo di = Directory.CreateDirectory(directoryPath);
			di.Attributes |= FileAttributes.Hidden;
		}

		public void Copy(string source, string destination)
		{
			File.Copy(source, destination);
		}

		public DirectoryPathAbsolute CurrentDirectory
		{
			get { return new DirectoryPathAbsolute(Directory.GetCurrentDirectory()); }
		}

        public virtual DirectoryPathAbsolute ExecutingDirectory
		{
			get { return new DirectoryPathAbsolute(AppDomain.CurrentDomain.BaseDirectory); }
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
			return Directory.GetDirectories(path);
		}

        public DirectoryPathAbsolute GetNuRoot
		{
			get { return ExecutingDirectory; }
		}
	}
}