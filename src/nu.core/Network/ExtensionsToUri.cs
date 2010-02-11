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
	using System.Net;

	public static class ExtensionsToUri
	{
		public static Uri For(this Uri location, RemoteCredentials credentials)
		{
			return location;
		}

		public static HttpWebRequest CreateWebRequest(this Uri location, DavVerb verb, RemoteCredentials credentials)
		{
			var request = (HttpWebRequest)WebRequest.Create(location.For(credentials));

			request.Credentials = credentials.For(location);
			request.Method = verb.ToString().ToUpperInvariant();

			return request;
		}
	}
}