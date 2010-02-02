namespace nu.Model.Project.Transformation
{
    public class ProjectTransformationPipeline : IProjectTransformationPipeline
    {
        private readonly ITransformationElement[] _elements;

        public ProjectTransformationPipeline(ITransformationElement[] elements)
        {
            _elements = elements;
        }

        public void Process(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            if(_elements != null)
            {
                foreach (ITransformationElement transformationElement in _elements)
                {
                    if (!transformationElement.Transform(templateManifest, environment, templateEnvironment))
                        break;
                }
            }
        }
    }
}