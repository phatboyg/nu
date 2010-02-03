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
namespace nu.core.Model.ArgumentParsing
{
	using System.Collections.Generic;

	public interface IArgumentMap
	{
		string Usage { get; }

		/// <summary>
		/// Applies the arguments to the specified object as they are enumerated
		/// </summary>
		/// <param name="obj">The object onto which the arguments should be applied</param>
		/// <param name="arguments">An enumerator of arguments being applied</param>
		IEnumerable<IArgument> ApplyTo(object obj, IEnumerable<IArgument> arguments);
	}
}