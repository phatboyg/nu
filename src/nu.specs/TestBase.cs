namespace nu.specs
{
    using System;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public abstract class TestBase
    {

        [SetUp]
        public void MainSetup()
        {
            _mocks = new MockRepository();
            During = new DuringHelper(_mocks);
            Setup();
        }

        [TearDown]
        public void MainTeardown()
        {
            _mocks = null;
            During = null;
            Teardown();
        }

        private MockRepository _mocks;

        protected virtual void Setup()
        {
        }
        protected virtual void Teardown()
        {
            
        }

        protected TType DynamicMock<TType>()
        {
            return _mocks.DynamicMock<TType>();
        }

        protected TType Stub<TType>()
        {
            return _mocks.Stub<TType>();
        }

        protected IDisposable Record()
        {
            return _mocks.Record();
        }

        protected IDisposable Playback()
        {
            return _mocks.Playback();
        }

        protected MockRepository Mocks
        {
            get { return _mocks; }
        }

        protected DuringHelper During;

        #region DuringHelper

        protected delegate void VoidDelegate();

        protected class DuringHelper
        {
            public DuringHelper(MockRepository repository)
            {
                _repository = repository;
            }

            private readonly MockRepository _repository;

            public void Record(VoidDelegate record)
            {
                using(_repository.Record())
                {
                    record.Invoke();
                }
            }

            public void Playback(VoidDelegate playback)
            {
                using (_repository.Playback())
                {
                    playback.Invoke();                    
                }
            }
        }

        #endregion
    }
}