// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.core.Remotes
{
    using System;
    using Configuration;
    using FileSystem;
    using Magnum.Logging;

    public class FileBasedLocalCatalogue :
        Catalogue
    {
        readonly FileSystem _fileSystem;
        static readonly ILogger _log = Logger.GetLogger<FileBasedLocalCatalogue>();
        File _file;
        bool _disposed;
        bool _touched;


        public FileBasedLocalCatalogue(FileSystem fileSystem, InstallationDirectory installLocation)
        {
            _fileSystem = fileSystem;
            _file = installLocation.GetChildFile("catalogue.json");
            RemoteCatalogues = ReadExistingDataFromFile(_file);
        }

        public ExternalLinks RemoteCatalogues { get; set; }

        ExternalLinks ReadExistingDataFromFile(File file)
        {
            _file = file;

            if (!file.Exists())
            {
                _log.Debug(x => x.Write("No existing catalogue file found: {0}", file.Name));

                return new ExternalLinks();
            }

			return new ExternalLinks(JsonUtil.Get<Remote[]>(file.ReadToEnd()) ?? new Remote[0]);
        }

        void WriteConfigurationToFile()
        {
			_log.Debug(x => x.Write("Saving configuration file: {0}", _file.Name));

            string json = JsonUtil.ToJson(RemoteCatalogues);

			// TODO: You need to write to the File interface, not using the _fileSystem
			_fileSystem.Write(_file.Name.GetPath(), json);
        }

        public bool Contains(string key)
        {
            return RemoteCatalogues.Contains(key);
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                if (_touched)
                    WriteConfigurationToFile();
            }

            _disposed = true;
        }


        public virtual void ForEach(Action<string, Uri> action)
        {
            foreach (var entry in RemoteCatalogues)
            {
                action(entry.Alias, entry.Address);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Uri this[string alias]
        {
            get
            {
                var entry = RemoteCatalogues.Get(alias);
                if (entry != null)
                    return entry.Address;

                throw new Exception(string.Format("no remote for {0}", alias));
            }

            set
            {
                RemoteCatalogues.Get(alias, x =>
                {
                    x.SetValue(value);
                    _touched = true;
                });
            }
        }

        public void Remove(string key)
        {
            RemoteCatalogues.Remove(key);
            _touched = true;
        }

        ~FileBasedLocalCatalogue()
        {
            Dispose(false);
        }
    }
}