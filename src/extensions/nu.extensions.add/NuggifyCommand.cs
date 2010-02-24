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
namespace nu.core.Commands
{
    using Configuration;
    using FileSystem;
    using Nugs.spec;

    public class NuggifyCommand :
        Command
    {
        readonly FileSystem _fileSystem;
        readonly CurrentWorkingDirectory _current;
        readonly string _name;
        readonly string _version;

        public NuggifyCommand(FileSystem fileSystem, CurrentWorkingDirectory current, string name, string version)
        {
            _fileSystem = fileSystem;
            _current = current;
            _name = name;
            _version = version;
        }

        public void Execute()
        {
            var man = new Manifest();
            man.Version = _version;
            man.Name = _name;

            foreach (var file in _current.GetFiles())
            {
                man.Files.Add(new ManifestEntry()
                    {
                        Name = file.Name.GetName()
                    });
            }

            var manStr = JsonUtil.ToJson(man);
            
            _fileSystem.Write(_current.GetChildFile("MANIFEST.json").Name.GetPath(), manStr);
        }
    }
}