using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using XF.Specs;

namespace nu.Model.Template
{
    [TestFixture]
    public class NVelocityAdapterSpecs : Spec
    {
        private ITemplateProcessor processor;
        private ITemplateContext context;

        protected override void Before_each_spec()
        {
            processor = new NVelocityTemplateProcessor();
            context = processor.CreateTemplateContext();
        }

        [Test]
        public void Should_be_able_to_render_a_simple_nvelocity_transformation_template()
        {
            string template = "Hello ${name}";
            context.Items.Add("name", "Nick");
            string output = processor.Process(template, context);

            Assert.That(output, Is.EqualTo("Hello Nick"));
        }

        [Test]
        public void Should_generate_a_valid_directory_path()
        {
            string template = string.Format("C:{0}Users{0}Nick{0}Desktop{0}demo{0}src{0}{1}", "${PathSeparator}", "${ProjectName}");
            context.Items.Add("ProjectName", "nu");
            context.Items.Add("PathSeparator", Path.DirectorySeparatorChar);
            string output = processor.Process(template, context);

            Assert.That(output, Is.EqualTo(@"C:\Users\Nick\Desktop\demo\src\nu"));
        }

    }
}