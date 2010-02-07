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
    using FilePaths;
    using Model.Files.Package;
    using Nugs;
    using Serialization;

    public class io
    {
        IFileSystem _fileSystem;
        NugRegistry _nugRegistry;

        public NugPackage Do(string nugName)
        {
            var nug = new NugPackage(nugName);
            _fileSystem.WorkWithTempDir((temp) =>
                {
                    var n = _nugRegistry.GetNug(nugName);

                    if (!n.Exists)
                        throw new Exception("cant find nug");

                    Zip.Unzip(n, temp);

                    //i now have the unzipped contents @ temp
                    var mani = temp.GetChildFileWithName("MANIFEST");
                    var maniS = File.ReadAllText(mani.Path);
                    //json it
                    var m = JsonUtil.Get<Manifest>(maniS);

                    nug.Name = m.Name;
                    nug.Version = m.Version;

                    foreach (var entry in m.Files)
                    {
                        nug.Files.Add(new NugFile
                            {
                                Name = entry.Name,
                                //whoa
                                File = new MemoryStream(File.ReadAllBytes(mani.GetBrotherFileWithName(entry.Name).Path))
                            });
                    }
                });
            return nug;
        }
    }
}