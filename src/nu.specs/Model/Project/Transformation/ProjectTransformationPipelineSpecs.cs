using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using XF.Specs;

namespace nu.Model.Project.Transformation
{
	using core.Model.Project.Transformation;

	public class When_executing_a_project_transformation_pipeline : Spec
    {
        [Test]
        public void Should_accept_transformation_elements()
        {
            ProjectTransformationPipeline pipeline = new ProjectTransformationPipeline(new AbstractTransformationElement[] { });
            Assert.That(pipeline, Is.Not.Null);
        }

        [Test]
        public void Should_process_pipeline_until_element_returns_false()
        {
            AbstractTransformationElement firstTrueElement = null;
            AbstractTransformationElement firstFalseElement = null;
            AbstractTransformationElement secondTrueElement = null;

            using (Record)
            {
                firstTrueElement = Mock<AbstractTransformationElement>();
                firstFalseElement = Mock<AbstractTransformationElement>();
                secondTrueElement = Mock<AbstractTransformationElement>();

                Expect.Call(firstTrueElement.Transform(null, null, null)).Return(true);
                Expect.Call(firstFalseElement.Transform(null, null, null)).Return(false);
            }
            using (Playback)
            {
                ProjectTransformationPipeline pipeline = new ProjectTransformationPipeline(
                    new AbstractTransformationElement[] { firstTrueElement, firstFalseElement, secondTrueElement });
                pipeline.Process(null, null, null);
            }
        }
    }
}