namespace nu.Model.Template
{
    using System;
    using System.Collections.Generic;

    public interface ITemplateContext
    {
        IDictionary<String, object> Items { get;}
    }
}