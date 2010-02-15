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
    using Configuration;
    using FileSystem;
    using spec;

    public class DotNetNugsDirectory :
        DotNetDirectory,
        NugsDirectory
    {

        public DotNetNugsDirectory(InstallationDirectory directory, NuConventions conventions)
            : base(directory.GetChildDirectory(conventions.NugsDirectoryName).Name)
        {
        }

        public NugPackage GetNug(string name)
        {
            var np = new NugPackage(name);

            Directory target = this.GetChildDirectory(name);

            var manifest = target.GetChildFile("MANIFEST.json");
            var manifestContent = manifest.ReadAllText();
            var m = JsonUtil.Get<Manifest>(manifestContent);

            np.Version = m.Version;

            foreach (var entry in m.Files)
            {
                var nf = new NugFile {Name = entry.Name};

                //TODO: ACK!
                target.GetChildFile(entry.Name).WorkWithStream(s =>
                    {
                        var ms = new System.IO.MemoryStream();
                        
                        
                            var buff = new byte[8048];
                            var size = buff.Length;

                            do
                            {
                                size = s.Read(buff, 0, buff.Length);
                            } while (size > 0);

                            nf.File = ms;
                        
                    });
                np.Files.Add(nf);
            }


            return np;
        }
    }
}