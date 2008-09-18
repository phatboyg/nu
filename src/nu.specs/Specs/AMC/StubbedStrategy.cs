using System;
using System.Reflection;
using Castle.MicroKernel;
using Rhino.Mocks;

namespace nu.Specs.AMC
{
    public class StubbedStrategy : AbstractMockingStrategy
    {
        #region Member Data

        private StandardMockingStrategy _default;

        #endregion

        #region StubbedStrategy()

        public StubbedStrategy(IAutoMockingRepository autoMock)
            : base(autoMock)
        {
            _default = new StandardMockingStrategy(autoMock);
        }

        #endregion

        public override object Create(CreationContext context, Type type)
        {
            object target = Mocks.Stub(type);
            AutoMock.AddService(type, target);
            return target;
        }
    }
}