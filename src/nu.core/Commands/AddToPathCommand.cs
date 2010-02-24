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
    using System;
    using FileSystem;
    using Magnum.Logging;

    public class AddToPathCommand :
        Command
    {
        readonly InstallationDirectory _installPoint;
        readonly ILogger _logger = Logger.GetLogger<AddToPathCommand>();

        public AddToPathCommand(InstallationDirectory installPoint)
        {
            _installPoint = installPoint;
        }

        //needs to request enhanced security
        public void Execute()
        {
            var old = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            _logger.Debug(x=>x.Write("PATH: {0}", old));
            var nupath = _installPoint.Name.GetPath();

            if(old.Contains(nupath))
                return; //aleady installed


            if (!old.EndsWith(";"))
                old += ";";

            if (nupath.Contains(" "))
                nupath = "\"" + nupath + "\"";
            _logger.Debug(x=>x.Write("setting PATH to '{0}'", old+nupath));

            Environment.SetEnvironmentVariable("PATH", old+nupath, EnvironmentVariableTarget.Machine);
        }
    }
}