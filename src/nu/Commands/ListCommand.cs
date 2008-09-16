namespace nu.Commands
{
    using System.Collections.Generic;
    using Model.ArgumentParsing;
    using nu.Model.Package;
    using Utility;

    [Command(Description = "List packages available in the local repository.")]
    public class ListCommand : ICommand
    {
       private readonly IPackageRepository _packageRepository;
       private readonly IConsole _console;

       public ListCommand(IPackageRepository packageRepository, IConsole console)
       {
          _packageRepository = packageRepository;
          _console = console;
       }

       public void Execute(IEnumerable<IArgument> arguments)
       {
          IEnumerable<Package> packages = _packageRepository.FindAll();

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