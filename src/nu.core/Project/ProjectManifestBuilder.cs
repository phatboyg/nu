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
    using System.Collections.Generic;
    using DTO;

    public class ProjectManifestBuilder
    {
        public Manifest Build(IProjectManifest projectManifest)
        {
            var manifest = new Manifest();
            manifest.folders = new List<FolderDTO>(projectManifest.Directories).ToArray();
            manifest.files = new List<FileDTO>(projectManifest.Files).ToArray();
            manifest.packages = new List<PackageDTO>(projectManifest.Packages).ToArray();
            return manifest;
        }

        public IProjectManifest Build(Manifest manifest)
        {
            var projectManifest = new ProjectManifest();
            foreach (FolderDTO folder in manifest.folders)
            {
                projectManifest.AddDirectory(folder);
            }
            foreach (FileDTO file in manifest.files)
            {
                projectManifest.AddFile(file);
            }
            foreach (PackageDTO package in manifest.packages)
            {
                projectManifest.AddPackage(package);
            }
            return projectManifest;
        }
    }
}