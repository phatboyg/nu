using System;

namespace nu.Model.Template
{
    public interface ITemplateProcessor
    {
        ITemplateContext CreateTemplateContext();
        string Process(String template, ITemplateContext context);
    }
}