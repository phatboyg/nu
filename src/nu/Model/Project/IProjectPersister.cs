using System;

namespace nu.Model.Project
{
    public interface IProjectPersister
    {
        void SaveProjectManifest(IProjectManifest manifest, String directory);
        bool ManifestExists(string directory);
    }
}