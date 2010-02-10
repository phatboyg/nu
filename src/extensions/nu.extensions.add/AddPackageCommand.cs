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
    using core.Commands;
    using core.Configuration;
    using Magnum.Logging;

    public class AddPackageCommand :
        Command
    {
        readonly ILogger _logger = Logger.GetLogger<AddPackageCommand>();
        readonly string _name;
        readonly GlobalConfiguration _globalConfiguration;

        public AddPackageCommand(string name, GlobalConfiguration globalConfiguration)
        {
            _name = name;
            _globalConfiguration = globalConfiguration;
        }

        public void Execute()
        {
            var nugFile = _globalConfiguration.NugsDirectory.GetNug(_name);
            _logger.Info(x => x.Write("looking for nug at {0}", nugFile.Name));
        }
    }
}