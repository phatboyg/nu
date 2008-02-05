namespace nu.Model.Package
{
   using System;
   using System.Collections.Generic;
   using System.Text;

   public interface IPackageCatalog
   {
      void Update(IEnumerable<CatalogEntry> catalogEntries);
      void Save();
   }
}
