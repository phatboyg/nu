namespace nu.Model.Project.Transformation
{
    public class ProjectTransformationPipeline : IProjectTransformationPipeline
    {
        private readonly AbstractTransformationElement[] _elements;

        public ProjectTransformationPipeline(AbstractTransformationElement[] elements)
        {
            _elements = elements;
        }

        public void Process(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            if(_elements != null)
            {
                foreach (AbstractTransformationElement transformationElement in _elements)
                {
                    if (!transformationElement.Transform(templateManifest, environment, templateEnvironment))
                        break;
                }
            }
        }
    }
}