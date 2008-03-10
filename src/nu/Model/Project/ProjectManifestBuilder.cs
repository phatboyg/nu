using System.Collections.Generic;
using nu.Model.Project;

namespace nu.Model.Project
{
    public class ProjectManifestBuilder
    {
        public Manifest Build(IProjectManifest projectManifest)
        {
            Manifest manifest = new Manifest();
            manifest.folders = new List<FolderDTO>(projectManifest.Directories).ToArray();
            manifest.files = new List<FileDTO>(projectManifest.Files).ToArray();
            manifest.packages = new List<PackageDTO>(projectManifest.Packages).ToArray();
            return manifest;
        }

        public IProjectManifest Build(Manifest manifest)
        {
            ProjectManifest projectManifest = new ProjectManifest();
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