namespace nu.Model.Template
{
    public interface ITemplateProcessor
    {
        void Transform(TransformationElement element);
        ITemplateContext Context{ get;}
    }
}