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
namespace nu.core.Project
{
    using nu.Model.Project;

    public class ProjectEnvironment : IProjectEnvironment
    {
        readonly bool _isTemplate;
        readonly string _projectDirectory;
        readonly string _projectName;

        public ProjectEnvironment(string projectName, string projectDirectory)
            : this(projectName, projectDirectory, false)
        {
        }

        public ProjectEnvironment(string projectName, string projectDirectory, bool isTemplate)
        {
            _projectName = projectName;
            _projectDirectory = projectDirectory;
            _isTemplate = isTemplate;
        }

        public string ProjectDirectory
        {
            get { return _projectDirectory; }
        }

        public string ProjectName
        {
            get { return _projectName; }
        }

        public bool IsTemplate
        {
            get { return _isTemplate; }
        }
    }
}