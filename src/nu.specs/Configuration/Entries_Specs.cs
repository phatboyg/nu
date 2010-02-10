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
namespace nu.Configuration
{
    using core.Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class Entries_Specs
    {
        [Test]
        public void Adding()
        {
            var entries = new Entries();
            entries.Get("name", x => x.SetValue("dru"));
            var e = entries.Get("name");
            Assert.AreEqual("dru", e.Value);
        }

        [Test]
        public void Removing()
        {
            var entries = new Entries();
            entries.Get("name", x => x.SetValue("dru"));
            entries.Remove("name");
            var e = entries.Get("name");
            Assert.IsNull(e);
        }

        [Test]
        public void Json()
        {
            var before = "[{\"Key\":\"name\",\"Value\":\"dru\"}]";
            var after = "[]";

            var entries = new Entries();
            entries.Get("name", x => x.SetValue("dru"));
            Assert.AreEqual(before, JsonUtil.ToJson(entries));
            entries.Remove("name");

            Assert.AreEqual(after, JsonUtil.ToJson(entries));
        }
    }
}