using System;
using System.Collections.Generic;
using System.Text;

namespace nu.Model.Package
{
   public interface IPackageSource
   {
      IList<CatalogEntry> FetchPackageCatalog();
   }
}
