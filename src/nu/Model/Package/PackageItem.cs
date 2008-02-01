namespace nu.Model.Package
{
    public class PackageItem
    {
        private readonly string _name;
        private readonly string _location;

        public PackageItem(string name, string location)
        {
            _name = name;
            _location = location;
        }


        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// lib / tools / etc
        /// </summary>
        public string Location
        {
            get { return _location; }
        }
    }
}