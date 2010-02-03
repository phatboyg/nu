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

	public interface IFileSystem
	{
		string CurrentDirectory { get; }
		string ExecutingDirectory { get; }
		char DirectorySeparatorChar { get; }
		string GetNuRoot { get; }
		bool Exists(string filePath);
		bool DirectoryExists(string directory);
		Stream Read(string filePath);
		String ReadToEnd(string filePath);
		void Write(string filePath, Stream file);
		void Write(string filePath, String content);
		void CreateDirectory(string directoryPath);
		void CreateHiddenDirectory(string directoryPath);
		void Copy(string source, string destination);
		bool IsRooted(string path);
		string Combine(string firstPath, string secondPath);
		string[] GetDirectories(string path);
		string GetTempFileName();
	}
}