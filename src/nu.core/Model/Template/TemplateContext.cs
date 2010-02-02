using System.Collections.Generic;

namespace nu.Model.Template
{
    public class TemplateContext : ITemplateContext
    {
        private readonly IDictionary<string, object> _context;

        public TemplateContext()
        {
            _context  = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Items
        {
            get { return _context; }
        }
    }
}