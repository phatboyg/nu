namespace Specs_for_ArgumentParser
{
    using System.Collections.Generic;
    using nu.Utility;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using XF.Specs;

    [TestFixture]
    public class When_parsing_the_argument_list : Spec
    {
        [Test]
        public void A_list_of_arguments_should_be_returned()
        {
            string[] args = new string[] {"one", "two", "three"};

            IArgumentParser parser = new ArgumentParser();

            IList<IArgument> arguments = parser.Parse(args);

            Assert.That(arguments, Is.Not.Null);

            Assert.That(arguments, Is.Not.Empty);

            Assert.That(arguments.Count, Is.EqualTo(args.Length));

            for (int index = 0; index < args.Length; index++)
                Assert.That(arguments[index].Value, Is.EqualTo(args[index]));
        }

        [Test]
        public void The_argument_list_should_be_adjustable()
        {
            string[] args = new string[] { "one", "two", "three" };

            IArgumentParser parser = new ArgumentParser();

            IList<IArgument> arguments = parser.Parse(args);

            arguments.RemoveAt(0);

            Assert.That(arguments[0].Value, Is.EqualTo("two"));
        }
    }
}