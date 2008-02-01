namespace nu.Commands
{
    using System.Collections.Generic;
    using nu.Model.Package;
    using Utility;

    [Command(Description = "List packages available in the local repository.")]
    public class ListCommand : ICommand
    {
       private readonly ILocalPackageRepository _localPackageRepository;
       private readonly IConsoleHelper _consoleHelper;

       public ListCommand(ILocalPackageRepository localPackageRepository, IConsoleHelper consoleHelper)
       {
          _localPackageRepository = localPackageRepository;
          _consoleHelper = consoleHelper;
       }

       public void Execute(IEnumerable<IArgument> arguments)
       {
          IEnumerable<Package> packages = _localPackageRepository.FindAll();

          if (packages == null)
          {
             _consoleHelper.WriteLine("No packages installed. Get to it!");
             return;
          }

          foreach(Package package in packages)
          {
             _consoleHelper.WriteLine(package.Name);
          }
       }
    }
}