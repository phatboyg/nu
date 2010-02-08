namespace nu.Model.Project
{
    public interface IProjectManifestRepository
    {
        IProjectManifest LoadProjectManifest(IProjectEnvironment environment);
        void SaveProjectManifest(IProjectManifest projectManifest, IProjectEnvironment environment);
        bool ManifestExists(IProjectEnvironment environment);
        string GetProjectDirectory(IProjectEnvironment environment);
        string GetProjectName(IProjectEnvironment environment);
        string GetManifestPath(IProjectEnvironment environment);
    }
}