namespace nu.Model.Package
{
   using System.Collections.Generic;

   public interface IPackageRepository
   {
      IEnumerable<Package> FindAll();

      Package FindCurrentVersionOf(string package);
   }
}