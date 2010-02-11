// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.core.Network
{
	using System;
	using System.IO;
	using System.Net;
	using FileSystem;

	public class DavRemoteDirectory :
		DavRemoteResource
		//,
		//RemoteDirectory

	{
		public DavRemoteDirectory(Uri location, RemoteCredentials credentials)
			: base(location, credentials)
		{
		}

//		public RemoteDirectory CreateDirectory(DirectoryName directoryName)
//		{
//			var buildUri = new UriBuilder(Location.Scheme, Location.Host, Location.Port, Path.Combine(Location.AbsolutePath, directoryName.GetName()));
//
//			HttpWebRequest request = buildUri.Uri.CreateWebRequest(DavVerb.MKCOL, Credentials);
//
//
//			try
//			{
//				using (WebResponse response = request.GetResponse())
//				{
//				}
//			}
//			catch (WebException ex)
//			{
//				if (!ex.Message.Contains("(405)")) // directory exists
//					throw;
//			}
//
//			return (IFolderResource)Client.Open(buildUri.Uri, Authentication, loadChildren);
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="fileName"></param>
//		/// <param name="content"></param>
//		/// <returns></returns>
//		public IFileResource PutFile(string fileName, Stream content)
//		{
//			if (content == null)
//				throw new ArgumentNullException("content");
//
//			if (string.IsNullOrEmpty(fileName))
//				throw new ArgumentNullException("fileName");
//
//			var buildUri = new UriBuilder(AbsoluteUri.Scheme, AbsoluteUri.Host, AbsoluteUri.Port, Path.Combine(AbsoluteUri.AbsolutePath, fileName));
//
//			var webRequest = (HttpWebRequest)WebRequest.Create(buildUri.Uri);
//
//			webRequest.Credentials = Authentication.Credentials;
//			webRequest.Method = "PUT";
//
//			// the contents to be uploaded
//			webRequest.ContentLength = content.Length;
//
//			using (Stream body = webRequest.GetRequestStream())
//			{
//				int bytesRead;
//				var data = new byte[128*1024];
//
//				while ((bytesRead = content.Read(data, 0, data.Length)) > 0)
//				{
//					body.Write(data, 0, bytesRead);
//				}
//
//				body.Close();
//			}
//
//			webRequest.GetResponse();
//
//			//Trace.WriteLine( "Open File " + buildUri.Uri.ToString( ) );
//			return (IFileResource)Client.Open(buildUri.Uri, Authentication, true);
//		}
//
//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="fileName"></param>
//		/// <param name="localFileName"></param>
//		/// <returns></returns>
//		public IFileResource PutFile(string fileName, string localFileName)
//		{
//			using (Stream content = new FileStream(localFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
//			{
//				try
//				{
//					return PutFile(fileName, content);
//				}
//				finally
//				{
//					content.Close();
//				}
//			}
//		}
	}
}