namespace nu.Model.Project.Transformation
{
    public interface IProjectTransformationPipeline
    {
        void Process(IProjectManifest templateManifest, IProjectEnvironment environment,
                     IProjectEnvironment templateEnvironment);
    }
}