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
namespace nu.util
{
	using core.Commands;
	using StructureMap;
	using StructureMap.Pipeline;

	public class StructureMapContainer :
		core.Container
	{
		readonly IContainer _container;
		readonly ObjectToDictionaryRegistry _converter = new ObjectToDictionaryRegistry();

		public StructureMapContainer(IContainer container)
		{
			_container = container;
		}

		public Command GetCommand<T>()
			where T : Command
		{
			return _container.GetInstance<T>();
		}

		public Command GetCommand<T>(object args)
			where T : Command
		{
			return _container.GetInstance<T>(new ExplicitArguments(_converter.Convert(args)));
		}

		public T GetInstance<T>()
		{
			return _container.GetInstance<T>();
		}

		public T GetInstance<T>(object args)
		{
			return _container.GetInstance<T>(new ExplicitArguments(_converter.Convert(args)));
		}
	}
}