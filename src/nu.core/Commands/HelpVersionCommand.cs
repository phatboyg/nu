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
namespace nu.core.Commands
{
	using Magnum.Logging;

	public class HelpVersionCommand :
		ICommand
	{
		readonly ILogger _log = Logger.GetLogger<HelpVersionCommand>();

		public void Execute()
		{
			_log.Info("Displays the version of nu that is installed");
			_log.Info("\t--verbose\tIncludes the version of each assembly that is registered");
		}
	}
}