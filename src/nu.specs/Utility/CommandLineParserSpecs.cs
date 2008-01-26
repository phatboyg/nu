namespace Specs_for_CommandLineParser
{
   using nu;
   using nu.specs;
   using NUnit.Framework;

   public class Person
   {
      [Argument(ArgumentType.AtMostOnce, HelpText = "", LongName = "name", ShortName = "n", DefaultValue = "blank")] public string Name;
   }

   [TestFixture]
   public class CommandLineParserTests : TestBase
   {
      [Test]
      public void Should_set_parameter_field_value_after_parsing_long_name()
      {
         Person p = new Person();
         string[] args = new string[] {"/name:Nick"};
         Parser.ParseArgumentsWithUsage(args, p);
         Assert.AreEqual("Nick", p.Name);
      }

      [Test]
      public void Should_set_parameter_field_value_after_parsing_short_name()
      {
         Person p = new Person();
         string[] args = new string[] {"/n:Nick"};
         Parser.ParseArgumentsWithUsage(args, p);
         Assert.AreEqual("Nick", p.Name);
      }

      [Test]
      public void Should_set_parameter_field_to_default_value_when_command_line_value_is_not_supplied()
      {
         Person p = new Person();
         string[] args = new string[] {string.Empty};
         Parser.ParseArgumentsWithUsage(args, p);
         Assert.AreEqual("blank", p.Name);
      }
   }
}