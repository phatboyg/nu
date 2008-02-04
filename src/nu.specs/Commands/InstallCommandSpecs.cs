namespace Specs_for_InstallCommand
{
   using System.Collections.Generic;
   using nu.Commands;
   using nu.Model.Package;
   using NUnit.Framework;
   using Rhino.Mocks;
   using XF.Specs;

   [TestFixture]
   public class When_installing_a_package_by_name : Spec
   {
      private InstallCommand _command;

      protected override void Before_each_spec()
      {
         _command = Create<InstallCommand>();
      }

      [Test]
      public void Get_a_list_of_sources()
      {
         using (Record)
         {
            Expect.Call(Get<IConfiguration>().PackageSources).Return(null);
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }

      [Test]
      public void Update_the_contents_of_each_source()
      {
         IPackageSource source1 = Mock<IPackageSource>();
         IPackageSource source2 = Mock<IPackageSource>();
         List<IPackageSource> sources = new List<IPackageSource>();
         sources.Add(source1);
         sources.Add(source2);

         using (Record)
         {
            SetupResult
               .For(Get<IConfiguration>().PackageSources)
               .Return(sources);
            Expect.Call(source1.FetchPackageCatalog()).Return(null);
            Expect.Call(source2.FetchPackageCatalog()).Return(null);
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }
   }
}