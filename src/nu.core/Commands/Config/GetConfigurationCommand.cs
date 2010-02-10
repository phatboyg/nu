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
namespace nu.core.Commands.Config
{
    using Configuration;
    using Magnum.Logging;

    public class GetConfigurationCommand :
        Command
    {
        readonly ProjectConfiguration _configuration;
        readonly string _key;
        readonly ILogger _log = Logger.GetLogger<GetConfigurationCommand>();

        public GetConfigurationCommand(string key, ProjectConfiguration configuration)
        {
            _configuration = configuration;
            _key = key;
        }

        public void Execute()
        {
            if (_configuration.Contains(_key))
            {
                _log.Debug(x => x.Write("Current configuration key '{0}' value: {1}", _key, _configuration[_key]));
                _log.Info(_configuration[_key]);
            }
            else
            {
                _log.Warn(x => x.Write("No configuration value is set for '{0}'", _key));
            }
        }
    }
}