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
namespace nu.core.Project.Transformation
{
    using FileSystem;
    using Templating;

    public abstract class AbstractTransformationElement :
        TransformationElement
    {
        protected const string DIRECTORY_KEY = "Directory";
        protected const string DIRECTORY_SEPARATOR_KEY = "PathSeparator";
        protected const string PROJECT_KEY = "ProjectName";

        public abstract bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment);

        public virtual TemplateContext BuildTemplateContext(FileSystem fileSystem,
                                                             TemplateProcessor templateProcessor, IProjectEnvironment environment)
        {
            TemplateContext context = templateProcessor.CreateTemplateContext();
            context.Items[PROJECT_KEY] = environment.ProjectName;
            context.Items[DIRECTORY_KEY] = environment.ProjectDirectory;
            context.Items[DIRECTORY_SEPARATOR_KEY] = fileSystem.DirectorySeparatorChar;
            return context;
        }
    }

    public interface TransformationElement
    {
        bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment);

        TemplateContext BuildTemplateContext(FileSystem fileSystem,
                                              TemplateProcessor templateProcessor, IProjectEnvironment environment);
    }
}