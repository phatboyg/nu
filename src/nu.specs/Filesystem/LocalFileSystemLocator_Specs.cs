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
namespace nu.Specs.Filesystem
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using core.FileSystem;
	using Magnum.TestFramework;
	using NUnit.Framework;

	[TestFixture]
	public class Creating_a_filename_from_a_string
	{
		string _location;
		FileName _fileName;

		[TestFixtureSetUp]
		public void Setup()
		{
			_location = Assembly.GetExecutingAssembly().Location;

			_fileName = FileName.GetFileName(_location);
		}

		[Test]
		public void Should_get_the_directory_name()
		{
			_fileName.GetDirectoryName().GetName().ShouldEqual("bin");
		}

		[Test]
		public void Should_retrieve_the_filename()
		{
			_fileName.GetName().ShouldEqual("nu.Specs.dll");
		}
	}

	[TestFixture]
	public class Passing_a_filename_to_the_locator
	{
		FileName _fileName;

		[TestFixtureSetUp]
		public void Setup()
		{
			_fileName = FileName.GetFileName(Assembly.GetExecutingAssembly().Location);

			FileSystemLocator locator = new LocalFileSystemLocator();

			core.FileSystem.File file = locator.GetFile(_fileName);

			file.Exists().ShouldBeTrue();

			file.Name.GetPath().ShouldEqual(_fileName.GetPath());
		}

		[Test]
		public void Getting_busy()
		{
			FileSystemLocator locator = new LocalFileSystemLocator();

			core.FileSystem.File file = locator.GetFile(FileName.GetFileName("nug.zip/manifest.json"));

			file.Name.GetName().ShouldEqual("manifest.json");
		}
	}

	public class LocalFileSystemLocator :
		FileSystemLocator
	{
		public core.FileSystem.File GetFile(FileName name)
		{
			core.FileSystem.File info = ResolveFileSystemInfo(name.Name);

			return info;
		}

		core.FileSystem.File ResolveFileSystemInfo(PathName name)
		{
			string root = Path.GetPathRoot(name.GetAbsolutePath());


			var di = new DirectoryInfo(root);

			string[] names = name.GetAbsolutePath().Substring(root.Length).Split('\\', '/');

			var directory = new DotNetDirectory(DirectoryName.GetDirectoryName(di.FullName));

			return ResolveFile(directory, names);
		}

		core.FileSystem.File ResolveFile(core.FileSystem.Directory directoryInfo, IEnumerable<string> children)
		{
			if (!children.Any())
				throw new InvalidOperationException("Unable to resolve file: " + directoryInfo.Name);

			string childName = children.First();

			Trace.WriteLine("Parsing out: " + childName);

			core.FileSystem.Directory info = directoryInfo.GetDirectories()
				.Where(x => string.Compare(x.Name.GetName(), childName, true) == 0)
				.SingleOrDefault();

			if (info != null)
			{
				Trace.WriteLine(string.Format("Found directory: {0}", info.Name));
				return ResolveFile(info, children.Skip(1));
			}

			core.FileSystem.File file = directoryInfo.GetFiles()
				.Where(x => string.Compare(x.Name.GetName(), childName, true) == 0)
				.SingleOrDefault();

			if (file == null)
				throw new InvalidOperationException("Could not get file: " + childName);


			if (!children.Skip(1).Any())
				return file;


			if (Path.GetExtension(file.Name.GetName()) == ".zip")
			{
				var zipFileDirectory = new ZipFileDirectory(file.Name.Name);
				return ResolveFile(zipFileDirectory, children.Skip(1));
			}

			throw new NotImplementedException();
		}
	}

	public interface FileSystemLocator
	{
		core.FileSystem.File GetFile(FileName name);
	}
}