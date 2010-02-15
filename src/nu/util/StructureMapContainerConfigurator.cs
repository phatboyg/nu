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
	using System;
	using core;
	using StructureMap;

	public class StructureMapContainerConfigurator :
		ContainerConfigurator
	{
		readonly IContainer _container;

		public StructureMapContainerConfigurator(IContainer container)
		{
			_container = container;
		}

		public void AddType<TInterface, TImplementation>()
			where TImplementation : TInterface
		{
			_container.Configure(x => x.For<TInterface>().Use<TImplementation>());
		}

		public void AddType(Type interfaceType, Type implementationType)
		{
			_container.Configure(x => x.For(interfaceType).Use(implementationType));
		}

		public void AddType<TInterface>(TInterface instance)
		{
			_container.Configure(x => x.For<TInterface>().Use(instance));
		}

		public void AddSingletonType<TInterface, TImplementation>()
			where TImplementation : TInterface
		{
			_container.Configure(x => x.For<TInterface>().Singleton().Use<TImplementation>());
		}

		public void AddSingletonType(Type interfaceType, Type implementationType)
		{
			_container.Configure(x => x.For(interfaceType).Singleton().Use(implementationType));
		}
	}
}