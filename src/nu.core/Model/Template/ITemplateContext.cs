namespace nu.Model.Template
{
    using System.Collections.Generic;

    public interface ITemplateContext
    {
        IDictionary<string, object> Items { get;}
    }
}