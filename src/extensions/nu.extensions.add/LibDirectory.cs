// Copyright 2007-2010 The Apache Software Foundation.
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
namespace nu.extensions.add
{
    using System;
    using System.Collections.Generic;
    using core.FileSystem;
    using core.Nugs;

    public interface LibDirectory :
        Directory
    {
    }

    public class DotNetLibDirectory :
        LibDirectory
    {
        public DotNetLibDirectory(NugDirectory nug)
        {
            
        }

        public DirectoryName Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Path
        {
            get { throw new NotImplementedException(); }
        }

        public Directory Parent
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasParentDir
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<File> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Directory> GetDirectories()
        {
            throw new NotImplementedException();
        }

        public Directory GetChildDirectory(string name)
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public File GetChildFile(string name)
        {
            throw new NotImplementedException();
        }

        public bool IsRoot()
        {
            throw new NotImplementedException();
        }
    }
}