namespace Specs_for_InjectCommand
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using nu;
   using nu.Commands;
   using nu.Model.Package;
   using nu.Utility;
   using NUnit.Framework;
   using Rhino.Mocks;
   using XF.Specs;

    [TestFixture]
    public class When_executing_the_inject_command_with_a_product_name_only : Spec
    {
        private InjectCommand command;
        private IEnumerable<IArgument> args;

        protected override void Before_each_spec()
        {
            command = Create<InjectCommand>();

            args = Mock<IEnumerable<IArgument>>();
        }

      [Test]
      public void Retrieve_the_package_by_product_name()
      {
          byte[] buffer = new byte[1];
          Stream s = new MemoryStream(buffer);
          s.WriteByte(1);

         using (Mocks.Record())
         {
            Expect.Call(Get<ILocalPackageRepository>().FindCurrentVersionOf("nunit")).Return(new Package("nunit"));
            Get<IFileSystem>().Write("nunit.txt", s);
         }
         using (Mocks.Playback())
         {
            command.Product = "nunit";
            command.Execute(args);
         }
      }
   }

    [TestFixture]
    public class When_executing_the_inject_command_without_a_product_name : Spec
    {
        private InjectCommand command;
        private IEnumerable<IArgument> args;

        protected override void Before_each_spec()
        {
            command = Create<InjectCommand>();
            args = Mock<IEnumerable<IArgument>>();
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Retrieve_the_package_by_product_name()
        {
            command.Execute(args);
        }
    }
}
