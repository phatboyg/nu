namespace nu.Commands
{
    using System.Collections.Generic;
    using nu.Model.Package;
    using Utility;

    [Command(Description = "List packages available in the local repository.")]
    public class ListCommand : ICommand
    {
       private ILocalPackageRepository _localPackageRepository;

       public ListCommand(ILocalPackageRepository localPackageRepository)
       {
          _localPackageRepository = localPackageRepository;
       }

       public void Execute(IEnumerator<IArgument> arguments)
       {
          
       }
    }
}