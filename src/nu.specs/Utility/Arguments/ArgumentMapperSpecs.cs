namespace Specs_for_ArgumentMapper
{
    using System.Collections.Generic;
    using nu.Model.ArgumentParsing;
    using nu.Model.ArgumentParsing.Exceptions;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using XF.Specs;

    [TestFixture]
    public class When_mapping_attributes_to_an_object : Spec
    {
        internal class StringClass
        {
            private string _value;

            [DefaultArgument]
            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        internal class BooleanClass
        {
            private bool _value;

            [DefaultArgument]
            public bool Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        internal class RequiredPropertyClass
        {
            private string _value;

            [DefaultArgument(Required = true)]
            public string Value
            {
                get{ return _value;}
                set{ _value = value;}
            }
        }

        [Test]
        public void A_boolean_value_should_be_converted_from_a_string()
        {
            List<IArgument> arguments = new List<IArgument>();
            arguments.Add(new Argument("true"));

            BooleanClass bc = new BooleanClass();

            IArgumentMapFactory mapFactory = new ArgumentMapFactory();

            IArgumentMap map = mapFactory.CreateMap(bc);

            map.ApplyTo(bc, arguments);

            Assert.That(bc.Value, Is.True);
        }

        [Test]
        public void A_string_value_should_be_stored_as_a_string()
        {
            List<IArgument> arguments = new List<IArgument>();
            arguments.Add(new Argument("one"));

            StringClass sc = new StringClass();

            IArgumentMapFactory mapFactory = new ArgumentMapFactory();

            IArgumentMap map = mapFactory.CreateMap(sc);

            map.ApplyTo(sc, arguments);

            Assert.That(sc.Value, Is.EqualTo("one"));
        }

        [Test, ExpectedException(typeof(MissingRequiredArgumentsException))]
        public void Should_throw_exception_when_required_argument_isnot_provided()
        {
            IArgumentMapFactory mapFactory = new ArgumentMapFactory();
            RequiredPropertyClass someClass = new RequiredPropertyClass();
            IArgumentMap map = mapFactory.CreateMap(someClass);
            map.ApplyTo(someClass, new List<IArgument>());
        }

    }
}