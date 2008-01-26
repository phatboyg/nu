namespace nu.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Package
    {
        private readonly string _name;
        private List<PackageItem> _packageItems = new List<PackageItem>();

        public Package(string name)
        {
            this._name = name;
        }

        public string Name
        {
            get { return this._name; }
        }

        public ReadOnlyCollection<PackageItem> Items
        {
            get { return new ReadOnlyCollection<PackageItem>(_packageItems); }
        }
    }
}