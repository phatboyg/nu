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
namespace nu.Specs.Filesystem
{
    using System;
    using System.IO;
    using ICSharpCode.SharpZipLib.Zip;
    using NUnit.Framework;

    [TestFixture]
    public class ZipPlay
    {
        //[Test]
        public void Metadata()
        {
            string zipFile = "test.zip";
            var zf = new ZipFile(zipFile);

            foreach (ZipEntry entr in zf)
            {
                Console.WriteLine("{0}:{1}", entr.Name, entr.IsDirectory);
            }
        }

        //[Test]
        public void ActualUse()
        {
            string outputDirectory = "out";
            string zipFile = "test.zip";
            var input = new ZipInputStream(File.OpenRead(zipFile));
            ZipEntry entry;

            while ((entry = input.GetNextEntry()) != null)
            {
                if (entry.IsDirectory)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(outputDirectory, entry.Name)));
                }
                else
                {
                    if (entry.Name.Length == 0)
                        return;

                    FileStream fs = File.Create(Path.Combine(outputDirectory, entry.Name));
                    int size = 2048;
                    var data = new byte[size];

                    while (true)
                    {
                        size = input.Read(data, 0, data.Length);

                        if (size == 0)
                            break;

                        fs.Write(data, 0, size);
                    }

                    fs.Close();
                }
            }
        }
    }
}