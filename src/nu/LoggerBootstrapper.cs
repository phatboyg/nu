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
	using System.Linq;
	using System.Reflection;
	using log4net;
	using log4net.Repository;
	using log4net.Repository.Hierarchy;
	using Magnum.Logging;
	using Magnum.Logging.Log4Net;

	public class LoggerBootstrapper
	{
		public ILogger Bootstrap(string name)
		{
			FileInfo fileInfo = GetLogConfigurationFileInfo();

			Log4NetLogProvider.Configure(fileInfo);

			ILogger logger = Magnum.Logging.Logger.GetLogger(name);

			Magnum.Logging.Logger.GetLogger<ContainerBootstrapper>().Debug(x => x.Write("Log configuration loaded: {0}", fileInfo.Name));

			return logger;
		}

		public static void SetLoggingLevel(string level)
		{
			var valid = new[] {"INFO", "DEBUG", "WARN", "ERROR", "FATAL"};
			level = level.ToUpperInvariant();

			if (!valid.Contains(level))
				throw new InvalidOperationException("An invalid logging level was specified");

			ILoggerRepository[] repositories = LogManager.GetAllRepositories();

			//Configure all loggers to be at the debug level.
			foreach (ILoggerRepository repository in repositories)
			{
				repository.Threshold = repository.LevelMap[level];
				var hier = (Hierarchy)repository;
				log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
				foreach (log4net.Core.ILogger logger in loggers)
				{
					((log4net.Repository.Hierarchy.Logger)logger).Level = hier.LevelMap[level];
				}
			}

			//Configure the root logger.
			var h = (Hierarchy)LogManager.GetRepository();
			log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
			rootLogger.Level = h.LevelMap[level];
		}

		static FileInfo GetLogConfigurationFileInfo()
		{
			string configurationFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			string configurationFile = Assembly.GetExecutingAssembly().GetName().Name + ".log4net.xml";
			return new FileInfo(Path.Combine(configurationFolder, configurationFile));
		}
	}
}