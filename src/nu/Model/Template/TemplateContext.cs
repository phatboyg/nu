using System;
using System.Collections;
using System.Collections.Generic;

namespace nu.Model.Template
{
    public class TemplateContext : ITemplateContext
    {
        private readonly IDictionary<String, Object> _context;

        public TemplateContext()
        {
            _context  = new Dictionary<String, Object>();
        }

        public IDictionary<string, object> Items
        {
            get { return _context; }
        }
    }
}