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
namespace nu.extensions.add
{
    using System;
    using core.Commands;
    using core.Configuration;
    using core.FileSystem;
    using Magnum.Logging;

    public class AddPackageCommand :
        Command
    {
        readonly ILogger _logger = Logger.GetLogger<AddPackageCommand>();
        readonly string _name;
        readonly GlobalConfiguration _globalConfiguration;
        readonly ProjectConfiguration _projectConfiguration;
        readonly FileSystem _fileSystem;

        public AddPackageCommand(string name, GlobalConfiguration globalConfiguration, ProjectConfiguration projectConfiguration, FileSystem fileSystem)
        {
            _name = name;
            _globalConfiguration = globalConfiguration;
            _projectConfiguration = projectConfiguration;
            _fileSystem = fileSystem;
        }
            
        public void Execute()
        {
            var package = _globalConfiguration.NugsDirectory.GetNug(_name);

            //TODO: should this be hidden behind another 'directory'?
            var lib = _projectConfiguration["project.librarydirectoryname"];
            var libDir  = _projectConfiguration.ProjectRoot.GetChildDirectory(lib);

            _logger.Debug(x=>x.Write("'lib' dir is located at '{0}'", libDir.Path));
            _fileSystem.CreateDirectory(libDir);
            var packageDir = libDir.GetChildDirectory(package.Name);
            _fileSystem.CreateDirectory(packageDir);

            foreach (var file in package.Files)
            {
                var writeTo = packageDir.GetChildFile(file.Name);
                _fileSystem.Write(writeTo.Path, file.File);
            }
        }
    }
}