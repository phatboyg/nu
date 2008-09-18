using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace nu.specs
{
   [TestFixture]
   public abstract class TestBase
   {
      #region Setup/Teardown

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

      #endregion

      private MockRepository _mocks;

      protected virtual void Setup() {}
      protected virtual void Teardown() {}

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

      protected class DuringHelper
      {
         private readonly MockRepository _repository;

         public DuringHelper(MockRepository repository)
         {
            _repository = repository;
         }

         public void Record(Action record)
         {
            using (_repository.Record())
            {
               record.Invoke();
            }
         }

         public void Playback(Action playback)
         {
            using (_repository.Playback())
            {
               playback.Invoke();
            }
         }
      }
   }
}