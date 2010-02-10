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
namespace nu.Specs.Model.Project.Transformation
{
    using core.Project.Transformation;
    using NUnit.Framework;
    using Rhino.Mocks;

    public class When_executing_a_project_transformation_pipeline
    {
        [Test]
        public void Should_accept_transformation_elements()
        {
            var pipeline = new ProjectTransformationPipeline(new AbstractTransformationElement[] {});
            Assert.That(pipeline, Is.Not.Null);
        }

        [Test]
        public void Should_process_pipeline_until_element_returns_false()
        {
            AbstractTransformationElement firstTrueElement = null;
            AbstractTransformationElement firstFalseElement = null;
            AbstractTransformationElement secondTrueElement = null;



            firstTrueElement = MockRepository.GenerateStub<AbstractTransformationElement>();
            firstFalseElement = MockRepository.GenerateStub<AbstractTransformationElement>();
            secondTrueElement = MockRepository.GenerateStub<AbstractTransformationElement>();

            Expect.Call(firstTrueElement.Transform(null, null, null)).Return(true);
            Expect.Call(firstFalseElement.Transform(null, null, null)).Return(false);

            var pipeline = new ProjectTransformationPipeline(
                new[] {firstTrueElement, firstFalseElement, secondTrueElement});
            pipeline.Process(null, null, null);

        }
    }
}