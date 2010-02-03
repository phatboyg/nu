namespace nu.Model.Package
{
   using System.Collections.Generic;
   using core.Model.Package;

	public interface IPackageRepository
   {
      IEnumerable<Package> FindAll();
      Package FindByName(string package);
   }
}