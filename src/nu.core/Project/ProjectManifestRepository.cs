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
    public class ProjectManifestRepository :
        IProjectManifestRepository
    {
        readonly IProjectManifestStore _store;

        public ProjectManifestRepository(IProjectManifestStore store)
        {
            _store = store;
        }

        public virtual IProjectManifest LoadProjectManifest(IProjectEnvironment environment)
        {
            return _store.Load(environment);
        }

        public virtual void SaveProjectManifest(IProjectManifest projectManifest, IProjectEnvironment environment)
        {
            _store.Save(environment, projectManifest);
        }

        public virtual bool ManifestExists(IProjectEnvironment environment)
        {
            return _store.Exists(environment);
        }

        public virtual string GetProjectDirectory(IProjectEnvironment environment)
        {
            return _store.GetProjectDirectory(environment);
        }

        public virtual string GetProjectName(IProjectEnvironment environment)
        {
            return _store.GetProjectName(environment);
        }

        public virtual string GetManifestPath(IProjectEnvironment environment)
        {
            return _store.GetManifestPath(environment);
        }
    }
}