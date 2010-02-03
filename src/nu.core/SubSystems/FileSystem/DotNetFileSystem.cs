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
	using Utility;

	public class DotNetFileSystem : IFileSystem
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

		public Stream Read(string filePath)
		{
			return new FileStream(filePath, FileMode.Open);
		}

		public string GetTempFileName()
		{
			return Path.GetTempFileName();
		}

		public String ReadToEnd(string filePath)
		{
			string contents;
			using (Stream stream = Read(filePath))
			{
				using (var reader = new StreamReader(stream))
				{
					contents = reader.ReadToEnd();
				}
			}
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

		public string CurrentDirectory
		{
			get { return Directory.GetCurrentDirectory(); }
		}

		public virtual string ExecutingDirectory
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
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

		public string GetNuRoot
		{
			get { return ExecutingDirectory; }
		}
	}
}