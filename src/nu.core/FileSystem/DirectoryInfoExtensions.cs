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
    using System.IO;
    using NDepend.Helpers.FileDirectoryPath;

    public static class DirectoryInfoExtensions
    {
        public static void CopyTo(this DirectoryPathAbsolute source, DirectoryPathAbsolute target)
        {
            foreach (FilePathAbsolute file in source.ChildrenFilesPath)
            {
                System.IO.File.Copy(file.Path, target.GetChildFileWithName(file.FileName).Path);
            }


            foreach (DirectoryPathAbsolute dir in source.ChildrenDirectoriesPath)
            {
                dir.CopyTo(target.GetChildDirectoryWithName(dir.DirectoryName));
            }
        }

        public static bool IsRoot(this DirectoryPathAbsolute source)
        {
            return source.DirectoryInfo.Root.Name.Replace("\\", "").Equals(source.Path);
        }

        public static void Create(this DirectoryPathAbsolute path)
        {
            if (!path.Exists)
                System.IO.Directory.CreateDirectory(path.Path);
        }

        public static void Create(this FilePathAbsolute path)
        {
            if(!path.Exists)
                System.IO.File.Create(path.Path).Dispose();
        }

        public static void Delete(this DirectoryPathAbsolute path)
        {
            if(path.Exists)
                System.IO.Directory.Delete(path.Path, true);
        }
    }
}