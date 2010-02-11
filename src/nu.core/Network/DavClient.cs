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
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Xml;
	using System.Xml.Linq;
	using Magnum.StreamExtensions;

	public class DavResource
	{
		public Uri Location { get; set; }
		public DateTime? CreateDate { get; set; }
		public DateTime? ModifyDate { get; set; }
		public string ETag { get; set; }
		public long? Length { get; set; }
		public bool Collection { get; set; }
	}

	public class DavClient
	{
		readonly RemoteCredentials _credentials;

		public DavClient(RemoteCredentials credentials)
		{
			_credentials = credentials;
		}

		public DavClient()
		{
		}

		public IEnumerable<DavResource> Query(Uri location)
		{
			return MakeRequest(location, DavVerb.PropFind);
		}

		IEnumerable<DavResource> MakeRequest(Uri uri, DavVerb verb)
		{
			using (var queryStream = new MemoryStream())
			{
				PrepareRequestXml(queryStream);

				long contentLength = queryStream.Length;

				HttpWebRequest request = PrepareWebRequest(uri, verb, contentLength);

				using (Stream s = request.GetRequestStream())
				{
					queryStream.WriteTo(s);
					s.Close();
				}

				using (var response = (HttpWebResponse)request.GetResponse())
				{
					using (Stream responseStream = response.GetResponseStream())
					{
						byte[] bytes = responseStream.ReadToEnd();

						Trace.WriteLine(Encoding.UTF8.GetString(bytes));

						using (var reader = new MemoryStream(bytes))
						{
							return ParseResponse(uri, reader);
						}
					}
				}
			}
		}

		IEnumerable<DavResource> ParseResponse(Uri location, Stream response)
		{
			using (XmlReader reader = XmlReader.Create(response))
			{
				XDocument xml = XDocument.Load(reader);

				IEnumerable<XElement> responses = from node in xml.Descendants(XName.Get("response", "DAV:")) select node;

				var each = from resp in responses
				           from props in resp.Descendants(XName.Get("propstat", "DAV:"))
				           from prop in resp.Descendants(XName.Get("prop", "DAV:"))
				           let url = resp.Element(XName.Get("href", "DAV:"))
				           let cdate = prop.Element(XName.Get("creationdate", "DAV:"))
				           let mdate = prop.Element(XName.Get("getlastmodified", "DAV:"))
				           let etag = prop.Element(XName.Get("getetag", "DAV:"))
				           let resourceType = prop.Element(XName.Get("resourcetype", "DAV:"))
				           let rt = resourceType.Element(XName.Get("collection", "DAV:"))
				           let leng = prop.Element(XName.Get("getcontentlength", "DAV:"))
				           select new DavResource
				           	{
				           		Location = (url == null ? location : GetUriOrNull(location, url.Value)), 
								CreateDate = (cdate == null ? (DateTime?)null : DateTime.Parse(cdate.Value)),
								ModifyDate = (mdate == null ? (DateTime?)null : DateTime.Parse(mdate.Value)),
								ETag = (etag == null ? null : etag.Value), 
								Length = (leng == null ? (long?)null : long.Parse(leng.Value)),
								Collection = (rt != null)
				           	};

				foreach (var item in each)
				{
					Trace.WriteLine("Found: " + item.Location ?? "Unknown");
					Trace.WriteLine("Modified: " + item.ModifyDate ?? "Unknown");
					Trace.WriteLine("Created: " + item.CreateDate ?? "Unknown");
					Trace.WriteLine("ETag: " + item.ETag ?? "(none)");
					Trace.WriteLine("Length: " + item.Length ?? "(unknown)");
					Trace.WriteLine("Resource Type: " + (item.Collection ? "Folder" : "File"));
				}

				return each;
			}
		}

		private Uri GetUriOrNull(Uri location, string value)
		{
			if(value == null)
				return location;

			string path = value;

			if(value.StartsWith("http"))
			{
				Uri uri = new Uri(value);
				path = uri.AbsolutePath;
			}

			var result = new UriBuilder(location.Scheme, location.Host, location.Port, path);

			return result.Uri;
		}


		HttpWebRequest PrepareWebRequest(Uri uri, DavVerb verb, long contentLength)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);

			if(_credentials != null)
				request.Credentials = _credentials.For(uri);

			request.Method = verb.ToString().ToUpperInvariant();
			request.ProtocolVersion = HttpVersion.Version10;
			request.Headers["Depth"] = "1";
			request.ContentType = "text/xml; charset=\"utf-8\"";
			request.ContentLength = contentLength;
			return request;
		}

		void PrepareRequestXml(MemoryStream mstream)
		{
			XNamespace d = "DAV:";

			var data = new XDocument(
				new XDeclaration("1.0", "utf-8", "yes"),
				new XElement(d + "propfind",
					new XAttribute(XNamespace.Xmlns + "d", "DAV:"),
					new XElement(d + "allprop")
					)
				);

			using (XmlWriter writer = XmlWriter.Create(mstream))
			{
				data.WriteTo(writer);
			}
		}
	}
}