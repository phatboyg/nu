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

	[Serializable]
	public class Entry
	{
		public Entry(string key)
		{
			Key = key;
		}

		public string Key { get; private set; }
		public string Value { get; private set; }

		public bool Equals(Entry other)
		{
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Equals(other.Key, Key);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(Entry))
				return false;
			return Equals((Entry)obj);
		}

		public override int GetHashCode()
		{
			return (Key != null ? Key.GetHashCode() : 0);
		}

		public void SetValue(string value)
		{
			if (Value == value)
				return;

			Value = value;
		}
	}
}