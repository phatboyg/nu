namespace nu.Model
{
    using System;

    public class PackageNotFoundException : Exception
    {
        private readonly string _packageName;

        public PackageNotFoundException(string packageName)
        {
            _packageName = packageName;
        }

        public string PackageName
        {
            get { return _packageName; }
        }
    }
}