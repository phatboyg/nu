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
    using core.Commands;
    using core.Configuration;
    using core.FileSystem;
    using core.Nugs;
    using Magnum.Logging;

    public class AddPackageCommand :
        Command
    {
        readonly FileSystem _fileSystem;
        readonly ILogger _logger = Logger.GetLogger<AddPackageCommand>();
        readonly string _name;
        readonly NugsDirectory _nugsDirectory;
        readonly ProjectConfiguration _projectConfiguration;
        readonly string _version;


        public AddPackageCommand(string name, NugsDirectory nugsDirectory, ProjectConfiguration projectConfiguration, FileSystem fileSystem)
            : this(name, null, nugsDirectory, projectConfiguration, fileSystem)
        {
        }

        public AddPackageCommand(string name, string version, NugsDirectory nugsDirectory, ProjectConfiguration projectConfiguration, FileSystem fileSystem)
        {
            _name = name;
            _version = version;
            _nugsDirectory = nugsDirectory;
            _projectConfiguration = projectConfiguration;
            _fileSystem = fileSystem;
        }

        public void Execute()
        {
            if (_projectConfiguration == null)
                throw new Exception("there is no project");

            _logger.Debug(f => f.Write(string.Format("Adding '{0}' version '{1}'", _name, _version)));
            //is the nug already installed?

            //if it is, what version?

            //install
            var package = _version == "MAX" ?
                _nugsDirectory.GetNug(_name) :
                _nugsDirectory.GetNug(_name, _version);

            //TODO: should be part of the project extension
            var libName = _projectConfiguration["project.librarydirectoryname"];
            var libDir = _projectConfiguration.ProjectRoot.GetChildDirectory(libName);
            _logger.Debug(x => x.Write("'lib' dir is located at '{0}'", libDir.Name));
            _fileSystem.CreateDirectory(libDir);
            //TODO: END

            var targetPackageDir = libDir.GetChildDirectory(package.NugName);
            _fileSystem.CreateDirectory(targetPackageDir);


            foreach (var file in package.GetFiles())
            {
                var fileWriteTo = targetPackageDir.GetChildFile(file.Name.GetName());
                _fileSystem.Copy(file.Name.GetPath(), fileWriteTo.Name.GetPath());
            }


            //TODO:this should be a part of the project extension
            /*
                _projectConfiguration.InstalledNugs.Add(new InstalledNugInformation()
                    {
                        Name = package.NugName,
                        Version = package.Version
             * });
             */
        }
    }
}