using System;
using Castle.MicroKernel;

namespace nu.Specs.AMC
{
    public class StandardMockingStrategy : AbstractMockingStrategy
    {
        #region StandardMockingStrategy()

        public StandardMockingStrategy(IAutoMockingRepository autoMock) : base(autoMock)
        {
        }

        #endregion

        public override object Create(CreationContext context, Type type)
        {
            return Mocks.CreateMock(type);
        }
    }
}