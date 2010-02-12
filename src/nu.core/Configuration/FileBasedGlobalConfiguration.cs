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
namespace nu.core.Configuration
{
    using System.Reflection;
    using FileSystem;
    using Magnum.Logging;

    public class FileBasedGlobalConfiguration :
        FileBasedConfiguration,
        GlobalConfiguration
    {
        readonly NuConventions _conventions;
        readonly ILogger _logger = Logger.GetLogger<FileBasedGlobalConfiguration>();

        public FileBasedGlobalConfiguration(DefaultsConfiguration defaults, FileSystem fileSystem, NuConventions conventions, InstallationDirectory install)
            : base(fileSystem, PathToMe(install, conventions))
        {
            Defaults = defaults;

            OnMissing = GetDefaultConfigurationValue;
            _conventions = conventions;
        }

        public Directory NuInstallDirectory
        {
            get
            {
            	return new DotNetDirectory(FileName.GetFileName(Assembly.GetEntryAssembly().Location).GetDirectoryName());
            }
        }

        DefaultsConfiguration Defaults { get; set; }

        public Directory WorkingDirectory
        {
            get { return FileSystem.GetCurrentDirectory(); }
        }

        public Directory ExtensionsDirectory
        {
            get { return NuInstallDirectory.GetChildDirectory(_conventions.ExtensionsDirectoryName); }
        }

        string GetDefaultConfigurationValue(string key)
        {
            _logger.Debug(x => x.Write("Falling back to DEFAULTS for key '{0}'", key));

            return Defaults[key];
        }

        static File PathToMe(InstallationDirectory install, NuConventions conventions)
        {
            return install.GetChildFile(conventions.ConfigurationFileName);
        }
    }
}