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
namespace nu.core.FileSystem.Zip
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using ICSharpCode.SharpZipLib.Zip;
	using Magnum.StreamExtensions;

	public class ZipFileDirectory :
		core.FileSystem.Directory
	{
		readonly IDictionary<string, ZippedDirectory> _directories = new Dictionary<string, ZippedDirectory>();
		readonly IDictionary<string, ZippedFile> _files = new Dictionary<string, ZippedFile>();
		Action _parse;

		public ZipFileDirectory(PathName pathName)
		{
			Name = DirectoryName.GetDirectoryName(pathName);

			_parse = Parse;

			_parse();
		}

		public core.FileSystem.Directory Parent
		{
			get
			{
				var fi = new FileInfo(Name.GetPath());
				return new DotNetDirectory(DirectoryName.GetDirectoryName(fi.DirectoryName));
			}
		}

		public bool HasParentDir
		{
			get { return true; }
		}

		public bool IsRoot()
		{
			return false;
		}

		public bool Exists()
		{
			return true;
		}

		public DirectoryName Name { get; private set; }

		public core.FileSystem.File GetChildFile(string name)
		{
			if (_files.ContainsKey(name))
				return _files[name];

			throw new InvalidOperationException("Could not find child element: " + name);
		}

		public core.FileSystem.Directory GetChildDirectory(string name)
		{
			if (_directories.ContainsKey(name))
				return _directories[name];

			throw new InvalidOperationException("Count not find child folder: " + name);
		}

		public IEnumerable<core.FileSystem.File> GetFiles()
		{
			return _files.Values.Cast<core.FileSystem.File>();
		}

		public IEnumerable<core.FileSystem.Directory> GetDirectories()
		{
			return _directories.Values.Cast<core.FileSystem.Directory>();
		}

		void Parse()
		{
			using (FileStream stream = File.OpenRead(Name.GetPath()))
			{
				var input = new ZipInputStream(stream);

				ZipEntry entry;
				while ((entry = input.GetNextEntry()) != null)
				{
					Trace.WriteLine("Reading Entry: " + entry.Name);

					if (entry.IsDirectory)
					{
					}
					else if (entry.IsFile)
					{
						byte[] data = input.ReadToEnd();

						Trace.WriteLine("Read: " + data.Length + " bytes");
					}
				}
			}

			_parse = () => { };
		}
	}
}