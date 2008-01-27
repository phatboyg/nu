namespace Specs_for_ArgumentMapper
{
    using System.Collections.Generic;
    using nu.Utility;
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

        [Test]
        public void A_string_value_should_be_stored_as_a_string()
        {
            List<IArgument> arguments = new List<IArgument>();
            arguments.Add(new Argument("one"));

            StringClass sc = new StringClass();

            IArgumentMapFactory mapFactory = new ArgumentMapFactory();

            IArgumentMap map = mapFactory.CreateMap(sc);

            map.ApplyTo(sc, arguments.GetEnumerator());

            Assert.That(sc.Value, Is.EqualTo("one"));
        }

        [Test]
        public void A_boolean_value_should_be_converted_from_a_string()
        {
            List<IArgument> arguments = new List<IArgument>();
            arguments.Add(new Argument("true"));

            BooleanClass bc = new BooleanClass();

            IArgumentMapFactory mapFactory = new ArgumentMapFactory();

            IArgumentMap map = mapFactory.CreateMap(bc);

            map.ApplyTo(bc, arguments.GetEnumerator());

            Assert.That(bc.Value, Is.True);
        }
    }
}