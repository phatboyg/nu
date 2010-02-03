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
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Model.ArgumentParsing;
	using nu.Model.Project;
	using nu.Model.Project.Transformation;
	using SubSystems.FileSystem;
	using Utility;

	[Command(Description = "Creates a new project")]
	public class NewProjectCommand : IOldCommand
	{
		readonly IConsole _console;
		readonly string _defaultTemplate;
		readonly IFileSystem _fileSystem;
		readonly IProjectTransformationPipeline _pipeline;
		readonly IProjectManifestRepository _projectManifestRepository;

		readonly string _rootTemplateDirectory;

		public NewProjectCommand(IFileSystem fileSystem, IProjectManifestRepository projectManifestRepository,
		                         IProjectTransformationPipeline pipeline, IConsole console, String rootTemplateDirectory,
		                         String defaultTemplate)
		{
			_console = console;
			_fileSystem = fileSystem;
			_projectManifestRepository = projectManifestRepository;
			_pipeline = pipeline;
			_rootTemplateDirectory = rootTemplateDirectory;
			_defaultTemplate = defaultTemplate;
		}

		[DefaultArgument(Required = true, Description = "The name of the project to create. (required)")]
		public string ProjectName { get; set; }

		[Argument(DefaultValue = "", Key = "d", AllowMultiple = false, Required = false, Description = "The directory to create the project.")]
		public string Directory { get; set; }

		[Argument(DefaultValue = "", Key = "t", AllowMultiple = false, Required = false, Description = "The template directory to use.")]
		public string Template { get; set; }


		public void Execute(IEnumerable<IArgument> arguments)
		{
			IProjectEnvironment projectEnvironment = new ProjectEnvironment(ProjectName, Directory);
			IProjectEnvironment templateEnvironment = new ProjectEnvironment(string.Empty,
				BuildTemplateDirectory(), true);

			try
			{
				if (!_projectManifestRepository.ManifestExists(projectEnvironment))
				{
					IProjectManifest templateManifest = _projectManifestRepository.LoadProjectManifest(templateEnvironment);
					_pipeline.Process(templateManifest, projectEnvironment, templateEnvironment);
					_projectManifestRepository.SaveProjectManifest(templateManifest, projectEnvironment);
				}
			}
			catch (FileNotFoundException ex)
			{
				_console.WriteError(ex.Message);
			}
		}

		string BuildTemplateDirectory()
		{
			return
				String.IsNullOrEmpty(Template)
					? _fileSystem.Combine(_rootTemplateDirectory, _defaultTemplate)
					: _fileSystem.Combine(_rootTemplateDirectory, Template);
		}
	}
}