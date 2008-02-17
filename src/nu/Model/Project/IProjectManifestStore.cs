using System;

namespace nu.Model.Project
{
    public interface IProjectManifestStore
    {
        IProjectManifest GetProjectManifestTemplate(IProjectEnvironment environment);
        IProjectManifest GetProjectManifest(IProjectEnvironment environment);
        void SaveProjectManifest(IProjectManifest manifest, IProjectEnvironment environment);
        bool ManifestExists(IProjectEnvironment environment);
    }
}