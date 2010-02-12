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
    using System.Collections.Generic;
    using System.IO;

    public class DotNetDirectory :
        Directory
    {
        public DotNetDirectory(DirectoryName directoryName)
        {
            Name = directoryName;
        }

        public IEnumerable<File> ChildrenFilesPath()
        {
                foreach (var file in System.IO.Directory.GetFiles(Path))
                {
                    yield return new DotNetFile(new AbsoluteFileName(file));
                }
                yield break;
           
        }

        public IEnumerable<Directory> ChildrenDirectories()
        {
                foreach (var dir in System.IO.Directory.GetDirectories(Path))
                {
                    yield return new DotNetDirectory(new AbsoluteDirectoryName(dir));
                }

                yield break;
            
        }

        public Directory GetChildDirectory(string name)
        {
            DirectoryName directoryName = Name.Combine(name);

            return new DotNetDirectory(directoryName);
        }

        public DirectoryName Name { get; private set; }

        public bool Exists()
        {
            return System.IO.Directory.Exists(Name.ToString());
        }

        public string Path
        {
            get { return Name.ToString(); }
        }

        public File GetChildFile(string name)
        {
            var path = System.IO.Path.Combine(Name.ToString(), name);
            if (System.IO.Path.IsPathRooted(path))
                return new DotNetFile(new AbsoluteFileName(path));

            return new DotNetFile(new RelativeFileName(path));
        }

        public Directory Parent
        {
            get
            {
                var di = new DirectoryInfo(Path);

                return new DotNetDirectory(new AbsoluteDirectoryName(di.Parent.FullName));
            }
        }

        public bool HasParentDir
        {
            get
            {
                var di = new DirectoryInfo(Path);
                return di.Parent != null;
            }
        }

        public bool IsRoot()
        {
            var di = new DirectoryInfo(Path);
            return di.Root.Name.Replace("\\", "").Equals(Path);
        }

        public override string ToString()
        {
            return Path;
        }
    }
}