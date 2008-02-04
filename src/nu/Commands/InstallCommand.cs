namespace nu.Commands
{
   using System.Collections.Generic;
   using nu.Model.Package;
   using Utility;

   public class InstallCommand : ICommand
   {
      private readonly IConfiguration _configuration;

      public InstallCommand(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      public void Execute(IEnumerable<IArgument> arguments)
      {
         IList<IPackageSource> sources = _configuration.PackageSources;
         if (sources == null) return;

         foreach(IPackageSource source in sources)
         {
            source.FetchPackageCatalog();
         }
      }
   }
}