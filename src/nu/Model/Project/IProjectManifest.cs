using System.Collections.Generic;

namespace nu.Model.Project
{
    public interface IProjectManifest
    {
        IList<FolderDTO> Directories { get; }
        IList<FileDTO> Files { get; }
        IList<PackageDTO> Packages { get; }
        FolderDTO FindFolder(string key);
    }
}