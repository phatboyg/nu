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