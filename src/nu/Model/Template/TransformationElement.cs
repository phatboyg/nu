namespace nu.Model.Template
{
    using System;

    public class TransformationElement
    {
        private readonly string _source;
        private readonly string _destination;

        public TransformationElement(String source, String destination)
        {
            _source = source;
            _destination = destination;
        }

        public string Source
        {
            get { return _source; }
        }

        public string Destination
        {
            get { return _destination; }
        }
    }
}