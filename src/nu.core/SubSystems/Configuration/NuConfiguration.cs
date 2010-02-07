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
namespace nu.core.SubSystems.Configuration
{
    using System;
    using System.Linq;
    using FileSystem;

    public class NuConfiguration :
        Config
    {
        readonly IFileSystem _fileSystem;

        public NuConfiguration(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            GlobalConfiguration = new Entries();
            ProjectConfiguration = new Entries();

            //where to initialize
        }

        /// <summary>
        /// Read out of the Global conf file
        /// </summary>
        public Entries GlobalConfiguration { get; set; }

        /// <summary>
        /// Read out of the Project conf file
        /// </summary>
        public Entries ProjectConfiguration { get; set; }

        /// <summary>
        /// Returns an enttry for the given key. Searching the project config before searchnig the global config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Entry GetEntryFor(string key)
        {
            if (ProjectConfiguration.Any(e => e.Key == key))
                return ProjectConfiguration.Single(e => e.Key == key);

            if (GlobalConfiguration.Any(e => e.Key == key))
                return GlobalConfiguration.Single(e => e.Key == key);

            throw new Exception(string.Format("no key of '{0}' found", key));
        }
    }
}