namespace nu.Model.Package
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Package
    {
        private readonly string _name;
        private readonly string _locationOnDisk;
        private readonly List<PackageItem> _packageItems = new List<PackageItem>(); //TODO: How to add to this?

        public Package(string name, string locationOnDisk)
        {
            _name = name;
            _locationOnDisk = locationOnDisk;
        }

        public string Name
        {
            get { return _name; }
        }

        public string LocationOnDisk
        {
            get { return _locationOnDisk; }
        }

        public ReadOnlyCollection<PackageItem> Items
        {
            get { return new ReadOnlyCollection<PackageItem>(_packageItems); }
        }
    }
}