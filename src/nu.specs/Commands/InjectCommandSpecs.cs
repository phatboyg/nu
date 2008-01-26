namespace Specs_for_InjectCommand
{
   using System;
   using nu.Commands;
   using nu.Model;
   using NUnit.Framework;
   using Rhino.Mocks;
   using XF.Specs;

   [TestFixture]
   public class When_executing_the_inject_command_with_a_product_name_only : Spec
   {
      private InjectCommand command;

      protected override void Before_each_spec()
      {
         command = Create<InjectCommand>();
      }

      [Test]
      public void Retrieve_the_package_by_product_name()
      {
         using (Mocks.Record())
         {
            Expect.Call(Get<ILocalPackageRepository>().FindCurrentVersionOf("nunit")).Return(new Package("nunit"));
         }
         using (Mocks.Playback())
         {
            command.Product = "nunit";
            command.Execute();
         }
      }
   }

   [TestFixture]
   public class When_executing_the_inject_command_without_a_product_name : Spec
   {
      private InjectCommand command;

      protected override void Before_each_spec()
      {
         command = Create<InjectCommand>();
      }

      [Test]
      [ExpectedException(typeof (ArgumentNullException))]
      public void Retrieve_the_package_by_product_name()
      {
         command.Execute();
      }
   }
}