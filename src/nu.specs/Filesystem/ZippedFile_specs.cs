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
    using core.FileSystem;
    using NUnit.Framework;

    [TestFixture]
    public class ZippedFile_specs
    {
        string _zippedFile = "nug.zip";

        ZipFileDirectory zf;

        [SetUp]
        public void SetUp()
        {
            zf = new ZipFileDirectory(new RelativeFileName(_zippedFile));
        }

        [Test]
        public void GetChildDirectory()
        {
            var c = zf.GetChildDirectory("test");
            Assert.AreEqual("nug.zip\\test", c.Path);
        }


        [Test]
        public void GetChildFileExists()
        {
            var c = zf.GetChildFile("MANIFEST.json");
            Assert.IsTrue(c.Exists());

            var c2 = zf.GetChildDirectory("lib").GetChildFile("yo.txt");
            Assert.IsTrue(c2.Exists());
        }

        [Test]
        public void ZippedDirectoryExists()
        {
            var c = zf.GetChildDirectory("lib");
            Assert.IsTrue(c.Exists());
            
        }

        [Test]
        public void GetChildFile()
        {
            var c = zf.GetChildFile("ho.txt");
            Assert.AreEqual("nug.zip\\ho.txt", c.Path);
        }

        [Test]
        public void PathWithRelative()
        {
            Assert.AreEqual("nug.zip", zf.Path);
        }

        [Test]
        public void ZipPathHelpers()
        {
            var result = ZippedPath.GetPathInsideZip("nug.zip\\test\\ho.txt");
            Assert.AreEqual("test/ho.txt", result);
        }

        [Test]
        public void MorePath()
        {
            var result = ZippedPath.GetZip("nug.zip\\test\\ho.txt");
            Assert.AreEqual("nug.zip", result);
        }
    }
}