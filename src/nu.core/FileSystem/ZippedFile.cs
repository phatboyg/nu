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
namespace nu.core.FileSystem
{
    using System;
    using System.IO;
    using System.Text;
    using ICSharpCode.SharpZipLib.Zip;

    public class ZippedFile :
        File
    {
        public ZippedFile(ZippedFileName name)
        {
            Name = name;
        }

        public FileName Name { get; set; }

        public bool Exists()
        {
            var innerPath = ZippedPath.GetPathInsideZip(Path);
            var zipFile = ZippedPath.GetZip(Path);
            var zf = new ZipFile(zipFile);

            return zf.Find(innerPath) != null;
        }

        public string ReadAllText()
        {
            var result = "";
            WorkWithStream(s=> result = Encoding.UTF8.GetString(((MemoryStream)s).ToArray()));
            //what encoding?
            return result;
        }

        public void WorkWithStream(Action<Stream> action)
        {
            var innerPath = ZippedPath.GetPathInsideZip(Path);
            var zipFile = ZippedPath.GetZip(Path);

            using (var ms = GetStreamFor(zipFile, innerPath))
            {
                action(ms);
            }
        }

        public string Path
        {
            get { return Name.ToString(); }
        }

        public Directory Parent
        {
            get
            {
                var path = ZippedPath.GetParentPath(Path);
                if(path.EndsWith(".zip"))
                {
                    return new ZipFileDirectory(new AbsoluteFileName(path));
                }
                else
                {
                    return new ZippedDirectory(new ZippedDirectoryName(path));
                }
            }
        }

        static MemoryStream GetStreamFor(string zipFile, string fileName)
        {
            var input = new ZipInputStream(System.IO.File.OpenRead(zipFile));
            ZipEntry zippy;
            while ((zippy = input.GetNextEntry()) != null)
            {
                if (zippy.Name.StartsWith(fileName))
                {
                    break;
                }
            }

            var ms = new System.IO.MemoryStream();
            var size = 2048;
            var data = new byte[size];

            while (true)
            {
                size = input.Read(data, 0, data.Length);

                if (size == 0)
                    break;

                ms.Write(data, 0, size);
            }
            return ms;
        }
    }

    public static class ZipFileExtensions
    {
        public static ZipEntry Find(this ZipFile file, string path)
        {
            ZipEntry result = null;

            foreach (ZipEntry entry in file)
            {
                if (entry.Name.StartsWith(path))
                {
                    result = entry;
                    break;
                }
            }
            return result;
        }
    }
}