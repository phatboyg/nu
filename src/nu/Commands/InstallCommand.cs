namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Model.ArgumentParsing;
    using nu.Model.Package;
    using Utility;

    [Command(Description = "Installs a package into a NU solution.")]
    public class InstallCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly IPackageRepository _packageRepository;
        private string _package;
        private IConsole _console;

        public InstallCommand(IPackageRepository packageRepository, IFileSystem fileSystem, IConsole console)
        {
            _packageRepository = packageRepository;
            _console = console;
            _fileSystem = fileSystem;
        }

        [Argument(Required = true)] 
        public string Package
        {
            get { return _package; }
            set { _package = value; }
        }

        public void Execute(IEnumerable<IArgument> arguments)
        {
            if (string.IsNullOrEmpty(Package))
                throw new ArgumentNullException("Product", "You must specify a package to install");

            _console.WriteLine("Injecting {0}", Package);

            Package pkg = _packageRepository.FindByName(Package);

            foreach(PackageItem item in pkg.Items)
            {
                WriteToProject(item);
            }

            _console.WriteLine("Finished Injecting {0}", Package);
        }

        private void WriteToProject(PackageItem item)
        {
            string target = item.Target; //lib, tools, src, etc
            string transformedTargetDir = "_someTransformationThing.Get(target)";

            _fileSystem.Copy(item.StorageLocation, Path.Combine(transformedTargetDir, item.FileName));
        }
    }
}