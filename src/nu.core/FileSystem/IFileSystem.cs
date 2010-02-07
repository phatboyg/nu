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
	using NDepend.Helpers.FileDirectoryPath;

    public interface IWellKnownLocations
    {
        
    }

    public interface IFileSystem
	{
        //well known paths
        DirectoryPathAbsolute WorkingDirectory { get; }
        DirectoryPathAbsolute InstallDirectory { get; }
        DirectoryPathAbsolute ProjectRoot { get; }
        DirectoryPathAbsolute ProjectNuDirectory { get; }
        DirectoryPath ExtensionsDirectory { get; }
        DirectoryPathAbsolute NugsDirectory { get; }

        //files
        FilePath ProjectConfig { get; }
        FilePath GlobalConfig { get; }


        //util
		char DirectorySeparatorChar { get; }
		bool FileExists(string filePath);
		bool DirectoryExists(string directoryPath);
		void Read(string filePath, Action<Stream> action);
		string ReadToEnd(string filePath);
		void Write(string filePath, Stream file);
		void Write(string filePath, string content);
		void CreateDirectory(string directoryPath);
		void CreateHiddenDirectory(string directoryPath);
		void Copy(string source, string destination);
		bool IsRooted(string path);
		string Combine(string firstPath, string secondPath);
		string[] GetDirectories(string path);
		string GetTempFileName();
        void WorkWithTempDir(Action<DirectoryPathAbsolute> tempAction);
	}
}