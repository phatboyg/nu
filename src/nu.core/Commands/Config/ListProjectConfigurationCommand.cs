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

    public class ListProjectConfigurationCommand :
        Command
    {
        readonly ProjectConfiguration _global;
        readonly ILogger _logger = Logger.GetLogger<ListProjectConfigurationCommand>();

        public ListProjectConfigurationCommand(ProjectConfiguration global)
        {
            _global = global;
        }

        public void Execute()
        {
            _global.ForEach((k, v) => _logger.Info(x => x.Write("'{0}':'{1}'", k, v)));
        }
    }
}