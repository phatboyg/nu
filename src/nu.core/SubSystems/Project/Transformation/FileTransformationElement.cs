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
namespace nu.core.Model.Project.Transformation
{
	using System.Globalization;
	using System.IO;
	using nu.Model.Project;
	using Resources;
	using SubSystems.FileSystem;
	using SubSystems.Templating;

    public class FileTransformationElement : 
        AbstractTransformationElement
	{
		readonly IFileSystem _fileSystem;
		readonly IProjectManifestRepository _manifestRepository;
		readonly ITemplateProcessor _processor;

		public FileTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem, IProjectManifestRepository manifestRepository)
		{
			_processor = processor;
			_fileSystem = fileSystem;
			_manifestRepository = manifestRepository;
		}

		public override bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
		{
			string rootDirectory = _manifestRepository.GetProjectDirectory(environment);
			ITemplateContext context = BuildTemplateContext(_fileSystem, _processor, environment);
			foreach (FileDTO file in templateManifest.Files)
			{
				string templateRoot = _manifestRepository.GetProjectDirectory(templateEnvironment);
				string fileTemplatePath = JoinTemplatePath(templateRoot, file.Source);
				string fileProcessedPath = _processor.Process(fileTemplatePath, context);

				if (!_fileSystem.FileExists(fileProcessedPath))
					throw new FileNotFoundException(
						string.Format(CultureInfo.CurrentUICulture, nuresources.FileTransformation_MissingFile, fileProcessedPath));

				string fileContent = _fileSystem.ReadToEnd(fileProcessedPath);
				string processedFileContent = _processor.Process(fileContent, context);

				string fileTemplateDestinationPath = JoinTemplatePath(rootDirectory, file.Destination);
				string fileProcessedDestinationPath = _processor.Process(fileTemplateDestinationPath, context);

				_fileSystem.Write(fileProcessedDestinationPath, processedFileContent);
				file.Source = string.Empty;
				file.Destination = fileProcessedDestinationPath;
			}
			return true;
		}

		public virtual string JoinTemplatePath(string firstPath, string secondPath)
		{
			return firstPath + secondPath;
		}
	}
}