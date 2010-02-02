namespace nu.core
{
    using System.IO;

    public static class StreamExtensions
    {
        public static void WriteToDisk(this Stream stream, string fileNameToWriteTo)
        {
            var buf = new byte[8192];


            using (
                var fileStream = new FileStream(fileNameToWriteTo, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = stream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        fileStream.Write(buf, 0, count);
                    }
                } while (count > 0); // any more data to read?
            }
        }
    }
}