namespace nu.Model.Package
{
   using System.Collections.Generic;

   public interface IConfiguration
   {
      IList<IPackageSource> PackageSources { get; }
   }
}
