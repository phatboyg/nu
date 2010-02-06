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
	using nu.Model.Project;
	using SubSystems.FileSystem;
	using SubSystems.Templating;

    public class FolderTransformationElement : AbstractTransformationElement
	{
		readonly IFileSystem _fileSystem;
		readonly IProjectManifestRepository _manifestRepository;
		readonly ITemplateProcessor _templateProcessor;


		public FolderTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem, IProjectManifestRepository manifestRepository)
		{
			_templateProcessor = processor;
			_fileSystem = fileSystem;
			_manifestRepository = manifestRepository;
		}

		public override bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
		{
			string rootDirectory = _manifestRepository.GetProjectDirectory(environment);
			ITemplateContext context = BuildTemplateContext(_fileSystem, _templateProcessor, environment);
			foreach (FolderDTO folder in templateManifest.Directories)
			{
				string folderTemplatePath = _fileSystem.Combine(rootDirectory, folder.Path);
				string folderProcessedPath = _templateProcessor.Process(folderTemplatePath, context);
				_fileSystem.CreateDirectory(folderProcessedPath);
				folder.Path = folderProcessedPath;
			}
			return true;
		}
	}
}