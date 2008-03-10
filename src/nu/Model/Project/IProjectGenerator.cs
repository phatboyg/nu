using nu.Model.Project;

namespace nu.Model.Project
{
    public interface IProjectGenerator
    {
        IProjectManifest Generate(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment);
    }
}