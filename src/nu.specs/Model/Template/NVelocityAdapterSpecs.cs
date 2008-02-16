using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using XF.Specs;

namespace nu.Model.Template
{
    [TestFixture]
    public class NVelocityAdapterSpecs : Spec
    {
        private ITemplateProcessor processor;

        protected override void Before_each_spec()
        {
            processor = new NVelocityAdapter();
        }

        [Test]
        public void Should_be_able_to_render_a_simple_nvelocity_transformation_template()
        {
            string template = "Hello ${name}";
            ITemplateContext context = processor.CreateTemplateContext();
            context.Items.Add("name", "Nick");
            string output = processor.Process(template, context);
            Assert.That(output, Is.EqualTo("Hello Nick"));
        }

        [Test]
        public void Should_work()
        {
            string template = "C://Users//Nick//Desktop//demo//src//${ProjectName}";
            ITemplateContext context = processor.CreateTemplateContext();
            context.Items.Add("ProjectName", "nu");
            string output = processor.Process(template, context);
            Assert.That(output, Is.EqualTo("C://Users//Nick//Desktop//demo//src//nu"));
        }

    }
}