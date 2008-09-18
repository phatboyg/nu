using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nu.Specs.AMC;
using Rhino.Mocks;

namespace nu.Specs
{
   public class Specification
   {
      private MockRepository _mocks;
      private AutoMockingContainer _container;

      public Specification()
      {
         _mocks = new MockRepository();
         _container = new AutoMockingContainer(_mocks);   
      }

      public T Create<T>()
      {
         return _container.Create<T>();
      }

      public T Dependency<T>()
         where T : class
      {
         return _container.Get<T>();
      }

      public T Mock<T>()
      {
         return _mocks.DynamicMock<T>();
      }

      public T Mock<T>(params object[] args)
      {
         return _mocks.DynamicMock<T>(args);
      }

   }
}
