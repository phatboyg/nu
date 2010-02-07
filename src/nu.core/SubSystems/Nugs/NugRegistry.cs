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
namespace nu.core.SubSystems.Nugs
{
    using System.IO;
    using FileSystem;
    using Model.Files.Package;
    using NDepend.Helpers.FileDirectoryPath;
    using Serialization;

    public class NugRegistry
    {
        readonly IFileSystem _fileSystem;

        public NugRegistry(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public FilePathAbsolute GetNug(string nugName)
        {
            var x = _fileSystem.NugsDirectory;
            var n = x.GetChildFileWithName(nugName);
            return n;
        }

        public NugPackage GetNugPackage(string name)
        {
            var path = _fileSystem.NugsDirectory.GetChildFileWithName(string.Format("{0}.nug", name));
            var target = new DirectoryPathAbsolute("");
            Zip.Unzip(path, target);
            var manifest = target.GetChildFileWithName("MANIFEST");
            var manifestContent = File.ReadAllText(manifest.Path);
            var m = JsonUtil.Get<Manifest>(manifestContent);
            var np = new NugPackage(m.Name);
            np.Version = m.Version;

            foreach (var entry in m.Files)
            {
                //get the file
                //load into a stream
                np.Files.Add(new NugFile()
                {
                    Name = entry.Name,
                    File = null //the file stream
                });
            }

            return np;
        }
    }
}