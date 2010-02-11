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
namespace nu.Specs.Configuration
{
    using NUnit.Framework;

    public class ProjectConfiguration_Specs
    {
        [Test]
        public void Project_Should_Fall_Back_On_Global()
        {
//            var glob = MockRepository.GenerateStub<GlobalConfiguration>();
//            var fs = MockRepository.GenerateStub<FileSystem>();
//
//            fs.Stub(f => f.ProjectConfig).Return(new DotNetFile(new RelativeFileName(@".\bob.txt")));
//            fs.Stub(f => f.FileExists(@".\bob.txt")).Return(false);
//            glob["name"] = "dru";
//
//            var proj = new FileBasedProjectConfiguration(fs, glob);
//
//            Assert.AreEqual("dru", proj["name"]);
        }
    }
}