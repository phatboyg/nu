namespace Specs_for_ListCommand
{
   using System.Collections.Generic;
   using nu.Commands;
   using nu.Model.Package;
   using nu.Utility;
   using NUnit.Framework;
   using Rhino.Mocks;
   using XF.Specs;

   [TestFixture]
   public class When_executing : Spec
   {
      private ListCommand _command;

      protected override void Before_each_spec()
      {
         _command = Create<ListCommand>();
      }

      [Test]
      public void Retrieve_packages_from_the_local_repository()
      {
         using (Record)
         {
            Expect.Call(Get<ILocalPackageRepository>().FindAll())
               .Return(new List<Package>());
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }

      [Test]
      public void Display_installed_packages()
      {
         List<Package> packages = new List<Package>();
         packages.Add(new Package("one"));
         packages.Add(new Package("two"));

         using (Record)
         {
            SetupResult.For(Get<ILocalPackageRepository>().FindAll())
               .Return(packages);
            Get<IConsoleHelper>().WriteLine(null);
            LastCall.IgnoreArguments().Repeat.Twice();
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }
   }

   [TestFixture]
   public class When_no_packages_are_installed : Spec
   {
      private ListCommand _command;

      protected override void Before_each_spec()
      {
         _command = Create<ListCommand>();
         SetupResult.For(Get<ILocalPackageRepository>().FindAll())
            .Return(null);
      }

      [Test]
      public void Display_a_message_indicating_no_packages_were_installed()
      {
         using (Record)
         {
            Get<IConsoleHelper>().WriteLine("No packages installed. Get to it!");
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }
   }
}