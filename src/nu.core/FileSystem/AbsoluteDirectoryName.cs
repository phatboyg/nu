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

	public class AbsoluteDirectoryName :
		DirectoryName
	{
		readonly string _path;

		public AbsoluteDirectoryName(string path)
		{
			if (!Path.IsPathRooted(path))
				throw new InvalidOperationException("The path must be an absolute path: " + path);

			_path = path;
		}

		public override string ToString()
		{
			return _path;
		}

		public override DirectoryName Combine(string path)
		{
			if (Path.IsPathRooted(path))
				return new AbsoluteDirectoryName(path);

			return new AbsoluteDirectoryName(Path.Combine(_path, path));
		}


        public override string GetName()
        {
            return System.IO.Path.GetDirectoryName(_path);
        }
	}
}