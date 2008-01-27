namespace nu.Commands
{
    using System;
    using System.Collections.Generic;
    using Model;
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
        //, 
            // HelpText = "The name of the product to inject.", 
            // LongName = "product",
            // ShortName = "p")] 
            public string Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public void Execute(IEnumerator<IArgument> arguments)
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