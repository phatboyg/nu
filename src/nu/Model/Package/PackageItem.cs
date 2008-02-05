namespace nu.Model.Package
{
    public class PackageItem
    {
        private readonly string _fileName;
        private readonly string _storageLocation;
        private string _target;

        public PackageItem(string fileName, string storageLocation)
        {
            _fileName = fileName;
            _storageLocation = storageLocation;
        }


        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// Where the item sits to be copied from
        /// </summary>
        public string StorageLocation
        {
            get { return _storageLocation; }
        }

        public string Target
        {
            get { return _target; }
        }
    }
}