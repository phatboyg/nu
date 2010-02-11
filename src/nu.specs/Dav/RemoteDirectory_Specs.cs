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
namespace nu.Specs.Dav
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using core.Network;
	using NUnit.Framework;

	[TestFixture]
	public class Connecting_to_a_remote_resource
	{
		[SetUp]
		public void Setup()
		{
			_credentials = new DavCredentials("username", "password");

			_uri = new Uri("http://masstransit.googlecode.com/svn/");
		}

		Uri _uri;
		RemoteCredentials _credentials;

		[Test]
		public void Should_query_google_code_okay()
		{
			var client = new DavClient();

			DavResource resource = client.Query(_uri).First();

			Assert.AreEqual(_uri, resource.Location);
		}

		[Test]
		public void Should_query_the_file_list()
		{
			var client = new DavClient();

			IEnumerable<DavResource> resources = from item in client.Query(new Uri("http://masstransit.googlecode.com/svn/branches/0.7/MassTransit/"))
			                                     where !item.Collection
			                                     where item.Location.ToString().EndsWith(".cs")
			                                     select item;

			Trace.WriteLine("Found " + resources.Count() + " files");
		}
	}
}