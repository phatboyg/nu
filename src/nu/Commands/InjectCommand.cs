namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using nu.Model.Package;
    using nu.Model.Project;
    using nu.Model.Template;
    using Utility;

    public class InjectCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILocalPackageRepository _localPackageRepository;
        private IProjectManifest _projectManifest;
        private string _product;

        public InjectCommand(ILocalPackageRepository localPackageRepository, IFileSystem fileSystem, IProjectManifest projectManifest)
        {
            _localPackageRepository = localPackageRepository;
            _projectManifest = projectManifest;
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

            Console.WriteLine("Injecting {0}", Product);

            nu.Model.Package.Package pkg = _localPackageRepository.FindCurrentVersionOf(Product);

            foreach(PackageItem item in pkg.Items)
            {
                WriteToProject(item);
            }
          
            Console.WriteLine("Finished Injecting {0}", Product);
        }

        private void WriteToProject(PackageItem item)
        {
            string target = item.Target; //lib, tools, src, etc
            string transformedTargetDir = "_someTransformationThing.Get(target)";

            _fileSystem.Copy(item.StorageLocation, Path.Combine(transformedTargetDir, item.FileName));
        }
    }
}