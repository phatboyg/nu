namespace nu.Commands
{
    using System.Collections.Generic;
    using nu.Model.Package;
    using Utility;

    [Command(Description = "List packages available in the local repository.")]
    public class ListCommand : ICommand
    {
       private readonly ILocalPackageRepository _localPackageRepository;
       private readonly IConsole _console;

       public ListCommand(ILocalPackageRepository localPackageRepository, IConsole console)
       {
          _localPackageRepository = localPackageRepository;
          _console = console;
       }

       public void Execute(IEnumerable<IArgument> arguments)
       {
          IEnumerable<Package> packages = _localPackageRepository.FindAll();

          if (packages == null)
          {
             _console.WriteLine("No packages installed. Get to it!");
             return;
          }

          foreach(Package package in packages)
          {
             _console.WriteLine(package.Name);
          }
       }
    }
}