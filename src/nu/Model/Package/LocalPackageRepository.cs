using System;
using System.Collections.Generic;
using System.Text;

namespace nu.Model.Package
{
   public class LocalPackageRepository : ILocalPackageRepository
   {
      public IEnumerable<Package> FindAll()
      {
         throw new NotImplementedException();
      }

      public Package FindCurrentVersionOf(string package)
      {
         throw new NotImplementedException();
      }
   }
}
