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
        public ZipFileDirectory(FileName fileName)
        {
            Name = new ZipFileName(fileName.ToString());
        }

        public Directory Parent
        {
            get
            {
                var fi = new FileInfo(Path);
                return new DotNetDirectory(new AbsoluteDirectoryName(fi.DirectoryName));
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
            get { return Name.ToString(); }
        }

        public File GetChildFile(string name)
        {
            var fileName = Name.ToString() + "\\" + name;

            return new ZippedFile(new ZippedFileName(fileName));
        }

        public Directory GetChildDirectory(string name)
        {
            //TODO: Ugh
            return new ZippedDirectory((ZippedDirectoryName)Name.Combine(name));
        }

        public IEnumerable<File> ChildrenFilesPath()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Directory> ChildrenDirectories()
        {
            throw new NotImplementedException();
        }
    }
}