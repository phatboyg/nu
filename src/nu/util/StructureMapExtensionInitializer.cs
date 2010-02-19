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
	using System.Collections.Generic;
	using core;
	using core.Commands;
	using Magnum.CommandLineParser;
	using Magnum.Monads.Parser;
	using StructureMap;
	using StructureMap.Pipeline;

	public class StructureMapExtensionInitializer :
		ExtensionInitializer
	{
		readonly ICommandLineElementParser<Command> _parser;

		readonly ContainerConfigurator _configurator;
		readonly core.Container _container;

		public StructureMapExtensionInitializer(ICommandLineElementParser<Command> parser, ContainerConfigurator configurator, core.Container containerAccess)
		{
			_parser = parser;
			_configurator = configurator;
			_container = containerAccess;
		}

		public void Add(Parser<IEnumerable<ICommandLineElement>, Command> parser)
		{
			_parser.Add(parser);
		}

		public Parser<IEnumerable<ICommandLineElement>, IArgumentElement> Argument()
		{
			return _parser.Argument();
		}

		public Parser<IEnumerable<ICommandLineElement>, IArgumentElement> Argument(string value)
		{
			return _parser.Argument(value);
		}

		public Parser<IEnumerable<ICommandLineElement>, IArgumentElement> Argument(Predicate<IArgumentElement> pred)
		{
			return _parser.Argument(pred);
		}

		public Parser<IEnumerable<ICommandLineElement>, IDefinitionElement> Definition()
		{
			return _parser.Definition();
		}

		public Parser<IEnumerable<ICommandLineElement>, IDefinitionElement> Definition(string key)
		{
			return _parser.Definition(key);
		}

		public Parser<IEnumerable<ICommandLineElement>, IDefinitionElement> Definitions(params string[] keys)
		{
			return _parser.Definitions(keys);
		}

		public Parser<IEnumerable<ICommandLineElement>, ISwitchElement> Switch()
		{
			return _parser.Switch();
		}

		public Parser<IEnumerable<ICommandLineElement>, ISwitchElement> Switch(string key)
		{
			return _parser.Switch(key);
		}

		public Parser<IEnumerable<ICommandLineElement>, ISwitchElement> Switches(params string[] keys)
		{
			return _parser.Switches(keys);
		}

		public Parser<IEnumerable<ICommandLineElement>, IArgumentElement> ValidPath()
		{
			return _parser.ValidPath();
		}

		public void AddType<TInterface, TImplementation>()
			where TImplementation : TInterface
		{
			_configurator.AddType<TInterface, TImplementation>();
		}

		public void AddType(Type interfaceType, Type implementationType)
		{
			_configurator.AddType(interfaceType, implementationType);
		}

		public void AddType<TInterface>(TInterface instance)
		{
			_configurator.AddType(instance);
		}

		public void AddSingletonType<TInterface, TImplementation>()
			where TImplementation : TInterface
		{
			_configurator.AddSingletonType<TInterface, TImplementation>();
		}

		public void AddSingletonType(Type interfaceType, Type implementationType)
		{
			_configurator.AddSingletonType(interfaceType, implementationType);
		}

		public Command GetCommand<T>()
			where T : Command
		{
			return _container.GetCommand<T>();
		}

		public Command GetCommand<T>(object args)
			where T : Command
		{
			return _container.GetCommand<T>(args);
		}

		public T GetInstance<T>()
		{
			return _container.GetInstance<T>();
		}

		public T GetInstance<T>(object args)
		{
			return _container.GetInstance<T>(args);
		}
	}
}