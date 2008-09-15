using System.IO;
using System.Collections;
using System.Collections.Generic;

using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.Exception;

namespace nu.Model.Template
{
    public class NVelocityTemplateProcessor : 
        VelocityEngine, ITemplateProcessor
    {
        public NVelocityTemplateProcessor()
        {
            Init();
        }

        public ITemplateContext CreateTemplateContext()
        {
            return new TemplateContext();
        }

        public string Process(string template, ITemplateContext context)
        {
            using(StringWriter writer = new StringWriter())
            {
                try
                {
                    Evaluate(CreateNVelocityContext(context.Items), writer, "nu", template);
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

        private static IContext CreateNVelocityContext(IDictionary<string, object> items)
        {
            IDictionary internalDictionary = new Hashtable();
            foreach (string key in items.Keys)
                internalDictionary.Add(key, items[key]);
            var context = new VelocityContext(new Hashtable(internalDictionary));
            return context;
        }
    }
}