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
    using System;
    using Configuration;
    using FileSystem;
    using Model.Files.Package;
    using NDepend.Helpers.FileDirectoryPath;

    public class NugRegistry
    {
        readonly FileSystem _fileSystem;
        readonly GlobalConfiguration _globalConfiguration;

        public NugRegistry(FileSystem fileSystem, GlobalConfiguration globalConfiguration)
        {
            _fileSystem = fileSystem;
            _globalConfiguration = globalConfiguration;
        }

        public NugPackage GetNugPackage(string name)
        {
            var np = new NugPackage(name);
            var path = _globalConfiguration.NugsDirectory.GetNug(name);
            _fileSystem.WorkWithTempDir(temp =>
                {
                    Directory target = new DotNetDirectory(new AbsoluteDirectoryName(temp.Path));
                    Zip.Unzip(path, target);
                    var manifest = target.GetChildFile("MANIFEST");
                    var manifestContent = manifest.ReadAllText();
                    var m = JsonUtil.Get<Manifest>(manifestContent);

                    np.Version = m.Version;

                    foreach (var entry in m.Files)
                    {
                        np.Files.Add(new NugFile
                        {
                            Name = entry.Name,
                            //whoa
                            File = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(manifest.Parent.GetChildFile(entry.Name).Path))
                        });
                    }
                });
            

            return np;
        }
    }
}