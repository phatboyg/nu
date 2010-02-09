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
namespace nu.core.Configuration
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Magnum;

	[Serializable]
	public class Entries :
		IEnumerable<Entry>
	{
		readonly IList<Entry> _items;

		public Entries()
		{
			_items = new List<Entry>();
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
		public IEnumerator<Entry> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		public Entry Get(string key)
		{
			return _items.SingleOrDefault(e => e.Key == key);
		}

		public void Get(string key, Action<Entry> setter)
		{
			var entry = _items.SingleOrDefault(e => e.Key == key);

			if(entry != null)
				setter(entry);
			else
			{
				entry = new Entry(key);
				_items.Add(entry);

				setter(entry);
			}
		}

		public void Remove(string key)
		{
			_items.Where(x => x.Key == key)
				.ToList()
				.Each(x => _items.Remove(x));
		}

		public bool Contains(string key)
		{
			return _items.Any(x => x.Key == key);
		}
	}
}