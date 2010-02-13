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
namespace nu.core.Templating
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using NVelocity;
    using NVelocity.App;
    using NVelocity.Context;
    using NVelocity.Exception;

    public class NVelocityTemplateProcessor :
        VelocityEngine,
        TemplateProcessor
    {
        public NVelocityTemplateProcessor()
        {
            Init();
        }

        public TemplateContext CreateTemplateContext()
        {
            return new NVelocityTemplateContext();
        }

        public string Process(nu.core.FileSystem.File template, TemplateContext context)
        {
            using (var writer = new StringWriter())
            {
                try
                {
                    Evaluate(CreateNVelocityContext(context.Items), writer, "nu", template.Path);
                }
                catch (ParseErrorException pe)
                {
                    return pe.Message;
                }
                catch (MethodInvocationException mi)
                {
                    return mi.Message;
                }
                return writer.ToString();
            }
        }

        static IContext CreateNVelocityContext(IDictionary<string, object> items)
        {
            IDictionary internalDictionary = new Hashtable();
            foreach (var key in items.Keys)
                internalDictionary.Add(key, items[key]);
            var context = new VelocityContext(new Hashtable(internalDictionary));
            return context;
        }
    }
}