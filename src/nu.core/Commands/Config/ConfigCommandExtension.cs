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
    using Magnum.Monads.Parser;

    public class ConfigCommandExtension :
        Extension
    {
        public void Initialize(ExtensionInitializer cli)
        {
            cli.Add(from config in cli.Argument("config")
                    from global in cli.Switch("global")
                    from get in cli.Switch("get")
                    from k in cli.Argument()
                    select cli.GetCommand<GetGlobalConfigurationCommand>(new {key = k.Id}));

            cli.Add(from config in cli.Argument("config")
                    from get in cli.Switch("get")
                    from k in cli.Argument()
                    select cli.GetCommand<GetConfigurationCommand>(new {key = k.Id}));

            cli.Add(from config in cli.Argument("config")
                    from global in cli.Switch("global")
                    from k in cli.Argument()
                    from v in cli.Argument()
                    select cli.GetCommand<SetGlobalConfigurationCommand>(new {key = k.Id, value = v.Id}));

            cli.Add(from config in cli.Argument("config")
                    from k in cli.Argument()
                    from v in cli.Argument()
                    select cli.GetCommand<SetConfigCommand>(new {key = k.Id, value = v.Id}));

            cli.Add(from config in cli.Argument("config")
                    from global in cli.Switch("global")
                    from list in cli.Switch("list")
                    select cli.GetCommand<ListGlobalConfigurationCommand>());
        }
    }
}