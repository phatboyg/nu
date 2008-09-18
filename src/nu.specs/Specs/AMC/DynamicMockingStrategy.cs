using System;
using Castle.MicroKernel;

namespace nu.Specs.AMC
{
    public class DynamicMockingStrategy : AbstractMockingStrategy
    {
        public DynamicMockingStrategy(IAutoMockingRepository autoMock) : base(autoMock)
        {
        }

        public override object Create(CreationContext context, Type type)
        {
            return Mocks.DynamicMock(type);
        }
    }
}