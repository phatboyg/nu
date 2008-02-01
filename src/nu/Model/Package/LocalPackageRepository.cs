using System;
using System.Collections.Generic;
using System.Text;

namespace nu.Model.Package
{
   public class LocalPackageRepository : ILocalPackageRepository
   {
      public IEnumerable<Package> FindAll()
      {
         List<Package> result = new List<Package>();
         result.Add(new Package("one"));
         result.Add(new Package("two"));
         return result;
      }

      public Package FindCurrentVersionOf(string package)
      {
         throw new NotImplementedException();
      }
   }
}
