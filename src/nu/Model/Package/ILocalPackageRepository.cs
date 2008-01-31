namespace nu.Model.Package
{
   using System.Collections.Generic;

   public interface ILocalPackageRepository
   {
      IEnumerable<Package> FindAll();

      Package FindCurrentVersionOf(string package);
   }
}