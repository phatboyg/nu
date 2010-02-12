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
namespace nu.core.Nugs
{
    using System.IO;
    using Configuration;
    using FileSystem;
    using Model.Files.Package;
    using Directory=nu.core.FileSystem.Directory;
    using File=System.IO.File;

    public class DotNetNugDirectory :
        DotNetDirectory,
        NugDirectory
    {
        readonly FileSystem _fileSystem;

        public DotNetNugDirectory(FileSystem fileSystem, InstallationDirectory directory, NuConventions conventions)
            : base(directory.GetChildDirectory(conventions.NugsDirectoryName).Name)
        {
            _fileSystem = fileSystem;
        }

        public NugPackage GetNug(string name)
        {
            var np = new NugPackage(name);
            Directory target = GetNugget(name);

            var manifest = target.GetChildFile("MANIFEST.json");
            var manifestContent = manifest.ReadAllText();
            var m = JsonUtil.Get<Manifest>(manifestContent);

            np.Version = m.Version;

            foreach (var entry in m.Files)
            {
                var nf = new NugFile() {Name = entry.Name};

                //TODO: ACK!
                target.GetChildFile(entry.Name).WorkWithStream(s=>nf.File = new MemoryStream(((MemoryStream)s).ToArray()));
                np.Files.Add(nf);
            }


            return np;
        }

        public Directory GetNugget(string name)
        {
            name += ".zip";
            if (IsADir(name))
                return new DotNetDirectory(base.Name.Combine(name));
            else
                return new ZipFileDirectory(new AbsoluteFileName(System.IO.Path.Combine(Path, name)));
        }

        public bool IsADir(string name)
        {
            return GetChildDirectory(name).Exists();
        }
    }
}