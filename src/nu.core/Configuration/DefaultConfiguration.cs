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

	public class DefaultConfiguration :
		Configuration
	{
		public DefaultConfiguration()
		{
			Entries = new Entries();
		}

		Entries Entries { get; set; }

		public virtual string this[string key]
		{
			get
			{
				Entry entry = Entries.Get(key);

				if (entry != null)
					return entry.Value;

				throw new ConfigurationException("No configuration entry for '" + key + "' was found");
			}

			set { throw new InvalidOperationException("Default configuration values cannot be changed"); }
		}

	    public void Remove(string key)
	    {
	        Entries.Remove(key);
	    }

	    public void Dispose()
		{
			Entries = null;
		}

		public bool Contains(string key)
		{
			return Entries.Contains(key);
		}

	    public void ForEach(Action<string, string> action)
	    {
	        foreach (var entry in Entries)
	        {
	            action(entry.Key, entry.Value);
	        }
	    }
	}
}