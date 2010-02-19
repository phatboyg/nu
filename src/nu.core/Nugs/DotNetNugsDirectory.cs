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
namespace nu.core.Nugs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Configuration;
    using FileSystem;
    using spec;

    public class DotNetNugsDirectory :
        DotNetDirectory,
        NugsDirectory
    {

        public DotNetNugsDirectory(InstallationDirectory directory, NuConventions conventions)
            : base(directory.GetChildDirectory(conventions.NugsDirectoryName).Name)
        {
        }


        public NugDirectory GetNug(string name)
        {
            return FindHighestVersion(name);
        }

        DotNetNugDirectory FindHighestVersion(string name)
        {
            var regex = new Regex(string.Format(@"{0}-(?<v>\d\.\d)", name));
            var versions = new List<string>();
            foreach (var directory in GetDirectories())
            {
                var m = regex.Match(directory.Name.GetName());
                if (!m.Success)
                    continue;
             
                var o = m.Groups["v"].Value;
                versions.Add(o);
            }

            var vv = versions.Max();
            var vvv = string.Format("{0}-{1}", name, vv);


            return new DotNetNugDirectory(GetChildDirectory(vvv).Name);
        }
    }
}