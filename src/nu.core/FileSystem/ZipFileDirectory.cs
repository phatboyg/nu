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
    using System.Collections.Generic;
    using System.IO;

    public class ZipFileDirectory :
        Directory
    {
    	public PathName PathName { get; set; }

    	public ZipFileDirectory(PathName pathName)
        {
        	PathName = pathName;
        }

    	public Directory Parent
        {
            get
            {
                var fi = new FileInfo(Path);
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
            return System.IO.File.Exists(Path);
        }

        public DirectoryName Name { get; set; }

        public string Path
        {
            get { return PathName.GetPath(); }
        }

        public File GetChildFile(string name)
        {
            return new ZippedFile(new ZipPathName(Path, name));
        }

        public Directory GetChildDirectory(string name)
        {
            //TODO: Ugh
            return new ZippedDirectory((ZipDirectoryName)Name.Combine(name));
        }

        public IEnumerable<File> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Directory> GetDirectories()
        {
            throw new NotImplementedException();
        }
    }
}