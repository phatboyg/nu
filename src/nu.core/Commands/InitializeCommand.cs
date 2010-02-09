namespace nu.core.Commands
{
    using System;
    using FileSystem;
    using Magnum.Logging;
    using Magnum.Monads.Parser;

    public class InitializeCommandExtension :
        Extension
    {
        public void Initialize(ExtensionInitializer cli)
        {

            cli.Add(from config in cli.Argument("init")
                    from path in cli.Argument()
                    select cli.GetCommand<InitializeCommand>(new { path = path.Id }));
        }
    }

    public class InitializeCommand :
        Command
    {
        readonly ILogger _log = Logger.GetLogger<GetGlobalConfigurationCommand>();
        readonly IFileSystem _fileSystem;
        readonly string _path;

        public InitializeCommand(string path, IFileSystem fileSystem)
        {
            _path = path;
            _fileSystem = fileSystem;
        }

        public void Execute()
        {
            if(_fileSystem.DirectoryExists(_path))
            {
                _log.Info(x=>x.Write("'{0}' exists", _path));
                var dir = _fileSystem.GetDirectory(_path);

                //this needs to be in a biz object
                var nu = dir.GetChildDirectoryWithName(".nu");
                nu.Create();
                nu.GetChildFileWithName("nu.conf").Create();
            }
            else
            {
                _log.Warn(x=>x.Write("Directory '{0}' does not exist.", _path));
            }
        }
    }
}