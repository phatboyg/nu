using nu.Model.Project;

namespace nu.Model.Project
{
    public interface IProjectGenerator
    {
        void Generate(IProjectManifest manifest, IProjectEnvironment environment);
    }
}