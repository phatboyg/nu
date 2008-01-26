namespace nu.Commands
{
   using System;
   using Model;

   public class InjectCommand : Command
   {
      private readonly ILocalPackageRepository _localPackageRepository;
      private readonly IFileSystem _fileSystem;

      [Argument(ArgumentType.Required, 
         HelpText = "The name of the product to inject.", 
         LongName = "product",
         ShortName = "p")] 
      public string Product;

      public InjectCommand(ILocalPackageRepository localPackageRepository, IFileSystem fileSystem)
      {
         _localPackageRepository = localPackageRepository;
         _fileSystem = fileSystem;
      }

      public override string Name
      {
         get { return "Inject"; }
      }

      public override void Execute()
      {
         if (string.IsNullOrEmpty(Product))
            throw new ArgumentNullException("Product", "You must specify a product to inject");

         Console.WriteLine("Injecting {0}", Product);

         Package pkg = _localPackageRepository.FindCurrentVersionOf(Product);
         // get dependencies?

         // write it to disk someplace
         _fileSystem.Write(null);

         Console.WriteLine("Finished Injecting {0}", Product);
      }
   }
}