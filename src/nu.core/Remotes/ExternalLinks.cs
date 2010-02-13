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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Magnum;

    [DebuggerDisplay("Remotes: {Count()}")]
    public class ExternalLinks :
        IEnumerable<Remote>
    {
        readonly IList<Remote> _items;

        public ExternalLinks()
        {
            _items = new List<Remote>();
        }

        public ExternalLinks(IEnumerable<Remote> entries)
        {
            _items = new List<Remote>(entries);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Remote> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public Remote Get(string alias)
        {
            return _items.SingleOrDefault(e => e.Alias == alias);
        }

        public void Get(string alias, Action<Remote> setter)
        {
            Remote entry = _items.SingleOrDefault(e => e.Alias == alias);

            if (entry != null)
                setter(entry);
            else
            {
                entry = new Remote(alias);
                _items.Add(entry);

                setter(entry);
            }
        }

        public void Remove(string alias)
        {
            _items.Where(x => x.Alias == alias)
                .ToList()
                .Each(x => _items.Remove(x));
        }

        public bool Contains(string key)
        {
            return _items.Any(x => x.Alias == key);
        }

        int Count()
        {
            return _items.Count;
        }
    }
}