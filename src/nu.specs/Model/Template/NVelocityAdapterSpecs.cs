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
namespace nu.Model.Template
{
	using System.IO;
	using core.Model.Template;
	using NUnit.Framework;
	using NUnit.Framework.SyntaxHelpers;
	using XF.Specs;

	[TestFixture]
	public class NVelocityAdapterSpecs : Spec
	{
		ITemplateProcessor processor;
		ITemplateContext context;

		protected override void Before_each_spec()
		{
			processor = new NVelocityTemplateProcessor();
			context = processor.CreateTemplateContext();
		}

		[Test]
		public void Should_be_able_to_render_a_simple_nvelocity_transformation_template()
		{
			string template = "Hello ${name}";
			context.Items.Add("name", "Nick");
			string output = processor.Process(template, context);

			Assert.That(output, Is.EqualTo("Hello Nick"));
		}

		[Test]
		public void Should_generate_a_valid_directory_path()
		{
			string template = string.Format("C:{0}Users{0}Nick{0}Desktop{0}demo{0}src{0}{1}", "${PathSeparator}", "${ProjectName}");
			context.Items.Add("ProjectName", "nu");
			context.Items.Add("PathSeparator", Path.DirectorySeparatorChar);
			string output = processor.Process(template, context);

			Assert.That(output, Is.EqualTo(@"C:\Users\Nick\Desktop\demo\src\nu"));
		}
	}
}