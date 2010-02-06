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
namespace nu.core.SubSystems.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NuConfiguration
    {
        /// <summary>
        /// Read out of the Global conf file
        /// </summary>
        public Entries GlobalConfiguration { get; set; }

        /// <summary>
        /// Read out of the Project conf file
        /// </summary>
        public Entries ProjectConfiguration { get; set; }

        /// <summary>
        /// Looks for a matching entry starting with the project conf then the global conf
        /// </summary>
        public IEnumerable<EntryPair> GetEntry(Func<Entry, bool> test)
        {
            //how can I get this to just return the 'p' then the 'g'
            //probably making this too hard
            return from p in ProjectConfiguration
                   from g in GlobalConfiguration
                   where test(p) || test(g)
                   select new EntryPair(p, g);
        }
    }
}