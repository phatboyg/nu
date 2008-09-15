namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using nu.Model.Package;
    using Utility;

    [Command(Description = "Injects a tool into the project")]
    public class InjectCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly IPackageRepository _packageRepository;
        private string _product;
        private IConsole _console;

        public InjectCommand(IPackageRepository packageRepository, IFileSystem fileSystem, IConsole console)
        {
            _packageRepository = packageRepository;
            _console = console;
            _fileSystem = fileSystem;
        }

        [Argument(Required = true)] 
        public string Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public void Execute(IEnumerable<IArgument> arguments)
        {
            if (string.IsNullOrEmpty(Product))
                throw new ArgumentNullException("Product", "You must specify a product to inject");

            _console.WriteLine("Injecting {0}", Product);

            Package pkg = _packageRepository.FindCurrentVersionOf(Product);

            foreach(PackageItem item in pkg.Items)
            {
                WriteToProject(item);
            }

            _console.WriteLine("Finished Injecting {0}", Product);
        }

        private void WriteToProject(PackageItem item)
        {
            string target = item.Target; //lib, tools, src, etc
            string transformedTargetDir = "_someTransformationThing.Get(target)";

            _fileSystem.Copy(item.StorageLocation, Path.Combine(transformedTargetDir, item.FileName));
        }
    }
}