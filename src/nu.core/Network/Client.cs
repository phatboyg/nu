//namespace nu.core.Network
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Diagnostics;
//	using System.IO;
//	using System.Net;
//	using System.Text;
//	using System.Xml;
//
//	public class Client
//	{
//		public static IResource Open(Uri remoteUri, IResourceAuthentication authentication, bool loadChildren)
//		{
//			FolderResource returnFolder = null;
//			IResource returnValue = null;
//			XmlDocument responseXml = new XmlDocument();
//
//			Debug.WriteLine(string.Format("Client.Open ( {0} ): Start", remoteUri));
//			using (MemoryStream mstream = new MemoryStream())
//			{
//
//				XmlWriterSettings settings = new XmlWriterSettings();
//				settings.Encoding = Encoding.UTF8;
//
//				using (XmlWriter writer = XmlWriter.Create(mstream))
//				{
//					writer.WriteStartDocument();
//					writer.WriteStartElement("d", "propfind", "DAV:");
//					writer.WriteStartElement("allprop", "DAV:");
//					writer.WriteEndDocument();
//
//					writer.Flush();
//					writer.Close();
//				}
//
//				Debug.WriteLine(string.Format("Client.Open ( {0} ): Create WebRequest", remoteUri));
//				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(remoteUri);
//
//				if (authentication != null)
//					webRequest.Credentials = authentication.Credentials;
//
//				webRequest.Method = "PROPFIND";
//				webRequest.Headers["Depth"] = (loadChildren ? "1" : "0");
//				webRequest.ContentType = "text/xml; charset=\"utf-8\"";
//
//				webRequest.ContentLength = mstream.Length;
//
//				using (Stream s = webRequest.GetRequestStream())
//				{
//					mstream.Position = 0;
//
//					s.Write(mstream.GetBuffer(), 0, (int)mstream.Length);
//					s.Close();
//				}
//
//				Debug.WriteLine(string.Format("Client.Open ( {0} ): Send WebRequest", remoteUri));
//				using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
//				{
//					Debug.WriteLine("Load Response");
//					responseXml.Load(webResponse.GetResponseStream());
//				}
//			}
//
//			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(responseXml.NameTable);
//			namespaceManager.AddNamespace("D", "DAV:");
//
//			XmlNodeList responseList = responseXml.SelectNodes("/D:multistatus/D:response", namespaceManager);
//
//			foreach (XmlNode responseNode in responseList)
//			{
//				XmlNode href = responseNode.SelectSingleNode("D:href", namespaceManager);
//				if (href != null)
//				{
//					string hrefPath = href.InnerText;
//
//					// Add check for innerText containing full url (IIS)
//					if (hrefPath.StartsWith("http"))
//					{
//						Uri uri = new Uri(href.InnerText);
//						hrefPath = uri.AbsolutePath;
//					}
//
//					UriBuilder responseUri = new UriBuilder(remoteUri.Scheme, remoteUri.Host, remoteUri.Port, hrefPath);
//
//					XmlNode resourceTypeNode = responseNode.SelectSingleNode("D:propstat/D:prop/D:resourcetype/D:collection", namespaceManager);
//
////					IResource resource;
////
////					if (resourceTypeNode != null)
////					{
////						IFolderResource folder = new FolderResource(responseUri.Uri, authentication, responseNode);
////						resource = folder;
////
////						folders.Add(folder);
////					}
////					else
////					{
////						IFileResource file = new FileResource(responseUri.Uri, authentication, responseNode);
////						resource = file;
////
////						files.Add(file);
////					}
////
////					if (PathsEqual(hrefPath, remoteUri.AbsolutePath))
////					{
////						returnValue = resource;
////						if (resource is FolderResource)
////						{
////							returnFolder = (FolderResource)resource;
////						}
////					}
//
////					XmlNodeList propertyNodes = responseNode.SelectNodes("D:propstat/D:prop/D:*", namespaceManager);
////					foreach (XmlNode propertyNode in propertyNodes)
////					{
////						switch (propertyNode.LocalName)
////						{
////							case "creationdate":
////								resource.CreateDate = DateTime.Parse(propertyNode.InnerText).ToUniversalTime();
////								break;
////
////							case "getlastmodified":
////								if (resource is IFileResource)
////									((IFileResource)resource).ModifyDate = DateTime.Parse(propertyNode.InnerText).ToUniversalTime();
////								break;
////
////							case "getcontentlength":
////								if (resource is IFileResource)
////									((IFileResource)resource).Length = long.Parse(propertyNode.InnerText);
////								break;
////
////							case "lockdiscovery":
////								if (resource is IFileResource)
////								{
////									XmlNode lockNode = propertyNode.SelectSingleNode("D:activelock/D:locktoken/D:href", namespaceManager);
////									if (lockNode != null)
////									{
////										string lockToken = lockNode.InnerText;
////										((IFileResource)resource).LockToken = lockToken;
////									}
////								}
////								break;
////
////							default:
////								break;
////						}
////					}
//				}
//			}
////			if (returnFolder != null)
////			{
////				returnFolder.Files = files.ToArray();
////				returnFolder.Folders = folders.ToArray();
////			}
////
//			return returnValue;
//		}
//
//		/// <summary>
//		/// Lock a remote resource
//		/// </summary>
//		/// <param name="resource"></param>
//		/// <returns></returns>
//		public static string Lock(IResource resource)
//		{
//			string ownerUri = "http://relayhealth.com/TransactionProcessor";
//			XmlDocument responseXml = new XmlDocument();
//
//			using (MemoryStream mstream = new MemoryStream())
//			{
//
//				XmlWriterSettings settings = new XmlWriterSettings();
//				settings.Encoding = Encoding.UTF8;
//
//				using (XmlWriter writer = XmlWriter.Create(mstream))
//				{
//					writer.WriteStartDocument();
//					writer.WriteStartElement("d", "lockinfo", "DAV:");
//
//					writer.WriteStartElement("lockscope", "DAV:");
//					writer.WriteStartElement("exclusive", "DAV:");
//					writer.WriteEndElement();
//					writer.WriteEndElement();
//
//					writer.WriteStartElement("locktype", "DAV:");
//					writer.WriteStartElement("write", "DAV:");
//					writer.WriteEndElement();
//					writer.WriteEndElement();
//
//					writer.WriteStartElement("owner", "DAV:");
//					writer.WriteElementString("href", "DAV:", ownerUri);
//
//					writer.WriteEndDocument();
//
//					writer.Flush();
//					writer.Close();
//				}
//
//				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(resource.AbsoluteUri);
//
//				webRequest.Credentials = resource.Authentication.Credentials;
//				webRequest.Method = "LOCK";
//				webRequest.Headers["Depth"] = "0";
//
//				webRequest.ContentType = "text/xml; charset=\"utf-8\"";
//				webRequest.ContentLength = mstream.Length;
//
//				using (Stream s = webRequest.GetRequestStream())
//				{
//					mstream.Position = 0;
//
//					s.Write(mstream.GetBuffer(), 0, (int)mstream.Length);
//					s.Close();
//				}
//				using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
//				{
//					responseXml.Load(webResponse.GetResponseStream());
//				}
//			}
//
//			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(responseXml.NameTable);
//			namespaceManager.AddNamespace("D", "DAV:");
//
//			XmlNode tokenNode = responseXml.SelectSingleNode("/D:prop/D:lockdiscovery/D:activelock/D:locktoken/D:href", namespaceManager);
//			if (tokenNode == null)
//				return null;
//
//			return tokenNode.InnerText;
//		}
//
//		/// <summary>
//		/// Unlocks a previously locked resource
//		/// </summary>
//		/// <param name="resource">The resource to unlock</param>
//		/// <param name="lockToken">The token issued when the resource was locked</param>
//		/// <returns></returns>
//		public static bool Unlock(IResource resource, string lockToken)
//		{
//			HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(resource.AbsoluteUri);
//
//			webRequest.Credentials = resource.Authentication.Credentials;
//			webRequest.Method = "UNLOCK";
//			webRequest.Headers["Lock-Token"] = "<" + lockToken + ">";
//
//			try
//			{
//				webRequest.GetResponse();
//
//				return true;
//			}
//			catch (WebException)
//			{
//				return false;
//			}
//		}
//
//		#region Private Static Methods
//
//		private static bool PathsEqual(string path1, string path2)
//		{
//			if (path1 == path2)
//				return true;
//
//			if (path1.EndsWith("/") && !path2.EndsWith("/"))
//				path2 += "/";
//
//			if (path2.EndsWith("/") && !path1.EndsWith("/"))
//				path1 += "/";
//
//			return (path1 == path2);
//		}
//
//		#endregion
//
//	}
//}