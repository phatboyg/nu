namespace nu.Model
{
   using System.Collections.Generic;

   public interface ILocalPackageRepository
   {
      IEnumerable<Package> FindAll();

      Package FindCurrentVersionOf(string package);
   }
}