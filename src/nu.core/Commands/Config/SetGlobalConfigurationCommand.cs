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

    public class SetGlobalConfigurationCommand :
        Command
    {
        readonly ILogger _log = Logger.GetLogger<SetGlobalConfigurationCommand>();
        readonly GlobalConfiguration _configuration;
        readonly string _key;
        readonly string _value;

        public SetGlobalConfigurationCommand(string key, string value, GlobalConfiguration configuration)
        {
            _key = key;
            _value = value;
            _configuration = configuration;
        }

        public void Execute()
        {
            _log.Debug(x => x.Write("Global configuration key '{0}' set to '{1}'", _key, _value));

            _configuration[_key] = _value;
        }
    }
}