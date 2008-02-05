namespace nu.Commands
{
   using System.Collections.Generic;
   using nu.Model.Package;
   using Utility;

   public class InstallCommand : ICommand
   {
      private readonly IConfiguration _configuration;
      private readonly IPackageCatalog _catalog;
      private readonly IConsole _console;

      public InstallCommand(IConfiguration configuration, IPackageCatalog catalog, IConsole console)
      {
         _configuration = configuration;
         _catalog = catalog;
         _console = console;
      }

      public void Execute(IEnumerable<IArgument> arguments)
      {
         IList<IPackageSource> sources = _configuration.PackageSources;
         if (sources == null) return;

         foreach(IPackageSource source in sources)
         {
            _catalog.Update(source.FetchPackageCatalog());
         }

         _catalog.Save();


      }
   }
}