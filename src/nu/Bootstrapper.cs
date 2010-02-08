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
	using System.IO;
	using System.Reflection;
	using core;
	using core.Commands;
	using core.Configuration;
	using core.SubSystems.FileSystem;
	using Magnum.InterfaceExtensions;
	using Magnum.Logging;
	using Magnum.Logging.Log4Net;
	using StructureMap;
	using StructureMap.Configuration.DSL;
	using StructureMap.Graph;

	public static class Bootstrapper
	{
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

		public static IContainer Bootstrap()
		{
			LoggerBootstrap();


			return ContainerBootstrap();
		}

		static void LoggerBootstrap()
		{
			string configurationFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			string configurationFile = Assembly.GetExecutingAssembly().GetName().Name + ".log4net.xml";
			var fileInfo = new FileInfo(Path.Combine(configurationFolder, configurationFile));

			Log4NetLogProvider.Configure(fileInfo);

			var logger = Logger.GetLogger(typeof(Program).Namespace);

			logger.Debug(x => x.Write("Log configuration loaded: {0}", fileInfo.Name));
		}

		static IContainer ContainerBootstrap()
		{
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

		static void ScanForExtensions(IContainer container, string path)
		{
			container.Configure(x =>
				{
					x.Scan(scan =>
						{
							scan.AssemblyContainingType<ICommand>();
							scan.AssembliesFromPath(path);

							scan.Convention<ExtensionConvention>();
						});
				});
		}
	}
}