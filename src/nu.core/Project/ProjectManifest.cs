using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace nu.Model.Project
{

    public class ProjectManifest : IProjectManifest
    {
        private readonly IList<FolderDTO> directories = new List<FolderDTO>();
        private readonly IList<FileDTO> files = new List<FileDTO>();
        private readonly IList<PackageDTO> packages = new List<PackageDTO>();

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

        public FolderDTO FindFolder(string key)
        {
            foreach (FolderDTO folderDTO in directories)
            {
                if (string.Compare(folderDTO.Key, key, StringComparison.CurrentCulture) == 0)
                    return folderDTO;
            }
            return null;
        }

    }
}