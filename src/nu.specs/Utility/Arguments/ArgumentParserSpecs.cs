namespace Specs_for_ArgumentParser
{
    using System.Collections.Generic;
    using nu.Model.ArgumentParsing;
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

        [Test]
        public void A_switch_style_argument_should_return_a_boolean_value()
        {
            string[] args = new string[] {"-v"};

            IArgumentParser parser = new ArgumentParser();

            IList<IArgument> arguments = parser.Parse(args);

            Assert.That(arguments.Count, Is.EqualTo(1));

            Assert.That(arguments[0].Key, Is.EqualTo("v"));

            Assert.That(arguments[0].Value, Is.EqualTo("true"));
        }

        [Test]
        public void A_switch_style_argument_should_return_false_if_it_is_disabled()
        {
            string[] args = new string[] { "-v-" };

            IArgumentParser parser = new ArgumentParser();

            IList<IArgument> arguments = parser.Parse(args);

            Assert.That(arguments.Count, Is.EqualTo(1));

            Assert.That(arguments[0].Key, Is.EqualTo("v"));

            Assert.That(arguments[0].Value, Is.EqualTo("false"));
        }

        [Test]
        public void A_switch_style_argument_with_a_value_should_be_a_key_value_pair()
        {
            string[] args = new string[] { "-v:one" };

            IArgumentParser parser = new ArgumentParser();

            IList<IArgument> arguments = parser.Parse(args);

            Assert.That(arguments.Count, Is.EqualTo(1));

            Assert.That(arguments[0].Key, Is.EqualTo("v"));

            Assert.That(arguments[0].Value, Is.EqualTo("one"));
        }

        [Test]
        public void A_switch_style_argument_with_a_value_should_be_a_key_and_no_value()
        {
            string[] args = new string[] { "-v:" };

            IArgumentParser parser = new ArgumentParser();

            IList<IArgument> arguments = parser.Parse(args);

            Assert.That(arguments.Count, Is.EqualTo(1));

            Assert.That(arguments[0].Key, Is.EqualTo("v"));

            Assert.That(arguments[0].Value, Is.EqualTo(""));
        }
        
        [Test]
        public void Should_parse_a_foward_slash_named_arguement()
        {
            string[] args = new string[]{"/d:c:\\projects"};
            IArgumentParser parser = new ArgumentParser();
            IList<IArgument> arguments = parser.Parse(args);

            Assert.That(arguments.Count, Is.EqualTo(1));
            Assert.That(arguments[0].Key, Is.EqualTo("d"));
            Assert.That(arguments[0].Value, Is.EqualTo("c:\\projects"));
        }

    }
}