namespace nu.core
{
    using System;
    using System.IO;
    using System.Net;

    public static class Http
    {
        public static Stream Get(Uri location)
        {
            var buf = new byte[8192];
            var request = (HttpWebRequest)WebRequest.Create(location);
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

    }
}