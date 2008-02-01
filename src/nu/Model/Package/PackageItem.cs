namespace nu.Model.Package
{
    public class PackageItem
    {
        private readonly string _fileName;
        private readonly string _storageLocation;

        public PackageItem(string fileName, string storageLocation)
        {
            _fileName = fileName;
            _storageLocation = storageLocation;
        }


        public string FileName
        {
            get { return _fileName; }
        }


        public string StorageLocation
        {
            get { return _storageLocation; }
        }
    }
}