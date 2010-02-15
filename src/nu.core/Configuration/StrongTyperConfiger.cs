// Copyright 2007-2010 The Apache Software Foundation.
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
    using System.Linq.Expressions;

    public class StrongTyperConfiger<T>
    {
        Entries _e;

        public StrongTyperConfiger(Entries e)
        {
            _e = e;
        }

        public T Build()
        {

            return (T)new object();
        }
    }

    public class EntriesWrapper<T>
    {
        Entries _e;

        public EntriesWrapper(Entries e)
        {
            _e = e;
        }

        public string GetValue(Expression<Func<T, object>> thing )
        {
            var o = thing.Body as MemberExpression;
            var p = o.Member.Name;
            var t = o.Member.DeclaringType.Name;
            var key = string.Format("{0}.{1}", t,p);
            return _e.Get(key).Value;
        }
    }
}