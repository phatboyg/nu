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
      public void Retrieve_the_contents_of_each_source()
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

      [Test]
      public void Update_the_package_catalog_with_contents_of_each_source()
      {
         IPackageSource source1 = Mock<IPackageSource>();
         List<IPackageSource> sources = new List<IPackageSource>();
         sources.Add(source1);
         List<CatalogEntry> catalogEntries = new List<CatalogEntry>();
         CatalogEntry catalogEntry = new CatalogEntry();
         catalogEntries.Add(catalogEntry);

         using (Record)
         {
            SetupResult
               .For(Get<IConfiguration>().PackageSources)
               .Return(sources);
            SetupResult.For(source1.FetchPackageCatalog()).Return(catalogEntries);
            Get<IPackageCatalog>().Update(catalogEntries);
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }

      [Test]
      public void Save_the_package_catalog()
      {
         IPackageSource source1 = Mock<IPackageSource>();
         List<IPackageSource> sources = new List<IPackageSource>();
         sources.Add(source1);

         using (Record)
         {
            SetupResult
               .For(Get<IConfiguration>().PackageSources)
               .Return(sources);
            Get<IPackageCatalog>().Save();
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }

      public void Find_the_catalog_entry_for_the_named_package()
      {
         
      }

      public void Obtain_the_source_from_the_catalog_entry()
      {
         
      }

      public void Use_the_source_to_get_the_full_package()
      {
         
      }

      public void Install_the_package_in_the_local_repository()
      {
         
      }
   }
}