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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using DTO;

    public class ProjectManifest : IProjectManifest
    {
        readonly IList<FolderDTO> directories = new List<FolderDTO>();
        readonly IList<FileDTO> files = new List<FileDTO>();
        readonly IList<PackageDTO> packages = new List<PackageDTO>();

        public IList<FolderDTO> Directories
        {
            get { return new ReadOnlyCollection<FolderDTO>(directories); }
        }

        public IList<FileDTO> Files
        {
            get { return new ReadOnlyCollection<FileDTO>(files); }
        }

        public IList<PackageDTO> Packages
        {
            get { return new ReadOnlyCollection<PackageDTO>(packages); }
        }

        public FolderDTO FindFolder(string key)
        {
            foreach (FolderDTO folderDTO in directories)
            {
                if (string.Compare(folderDTO.Key, key, StringComparison.CurrentCulture) == 0)
                    return folderDTO;
            }
            return null;
        }

        public IProjectManifest AddDirectory(FolderDTO folderDTO)
        {
            directories.Add(folderDTO);
            return this;
        }

        public IProjectManifest AddFile(FileDTO fileDTO)
        {
            files.Add(fileDTO);
            return this;
        }

        public IProjectManifest AddPackage(PackageDTO packageDTO)
        {
            packages.Add(packageDTO);
            return this;
        }
    }
}