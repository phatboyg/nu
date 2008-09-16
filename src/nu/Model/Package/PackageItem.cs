namespace nu.Model.Package
{
    public class PackageItem
    {
        private readonly string _fileName;
        private readonly string _storageLocation;
        private readonly string _target;

        public PackageItem(string fileName, string storageLocation, string target)
        {
            _fileName = fileName;
            _target = target;
            _storageLocation = storageLocation;
        }


        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// Where in the package directory the file is stored to be copied from
        /// </summary>
        public string StorageLocation
        {
            get { return _storageLocation; }
        }

        //TODO: this should be some kind of enumeration
        /// <summary>
        /// lib, tools, stuff etc
        /// </summary>
        public string Target
        {
            get { return _target; }
        }
    }
}