namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using nu.Model.Package;
    using nu.Model.Template;
    using Utility;

    public class InjectCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILocalPackageRepository _localPackageRepository;
        private string _product;

        public InjectCommand(ILocalPackageRepository localPackageRepository, IFileSystem fileSystem)
        {
            _localPackageRepository = localPackageRepository;
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

            Package pkg = _localPackageRepository.FindCurrentVersionOf(Product);
            
            foreach(PackageItem item in pkg.Items)
            {
                WriteToProject(item);
            }
          
            Console.WriteLine("Finished Injecting {0}", Product);
        }

        private void WriteToProject(PackageItem item)
        {
            //some transform stuff here?
            // each package item has one or more targets or symbols
            TransformationElement elem = null;
            _fileSystem.Copy(elem.Source, elem.Destination);
        }
    }
}