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
namespace nu
{
	using System;
	using core;
	using core.Commands;
	using core.Configuration;
	using core.SubSystems.FileSystem;
	using Magnum.InterfaceExtensions;
	using Magnum.Logging;
	using StructureMap;
	using StructureMap.Configuration.DSL;
	using StructureMap.Graph;

	public class ContainerBootstrapper
	{
		readonly ILogger _log = Logger.GetLogger<ContainerBootstrapper>();

		public IContainer Bootstrap()
		{
			_log.Debug("Bootstrapping container");

			IContainer container = new Container(x =>
				{
					x.For<GlobalConfiguration>()
						.Singleton()
						.Use<GlobalFileBasedConfiguration>();

//					x.For<ProjectConfiguration>()
//						.Singleton()
//						.Use<ProjectFileBasedConfiguration>();

					x.For<IPath>()
						.Singleton()
						.Use<PathAdapter>();

					x.For<IFileSystem>()
						.Singleton()
						.Use<DotNetFileSystem>();
				});

			ScanForExtensions(container, container.GetInstance<IFileSystem>().ExtensionsDirectory.Path);

			return container;
		}

		class ExtensionConvention :
			IRegistrationConvention
		{
			readonly ILogger _log = Logger.GetLogger<ExtensionConvention>();

			public void Process(Type type, Registry registry)
			{
				if (type == typeof(Extension))
					return;

				if (type.Implements<Extension>())
				{
					_log.Debug(x => x.Write("Loading extension: {0} ({1})", type.Name, type.Assembly.GetName().Name));

					registry.AddType(typeof(Extension), type);
				}
			}
		}

		void ScanForExtensions(IContainer container, string path)
		{
			container.Configure(x =>
				{
					x.Scan(scan =>
						{
							_log.Debug(d => d.Write("Scanning {0} for extensions", path));

							scan.AssemblyContainingType<ICommand>();
							scan.AssembliesFromPath(path);

							scan.Convention<ExtensionConvention>();
						});
				});
		}
	}
}