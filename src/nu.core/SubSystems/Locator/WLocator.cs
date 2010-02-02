namespace nu
{
   using System.Configuration;
   using Castle.Windsor;

   public static class WLocator
   {
      private static IWindsorContainer _container;

      public static IWindsorContainer Container
      {
         get
         {
            CreateContainer();
            return _container;
         }
         set { _container = value; }
      }

      public static T Resolve<T>()
      {
         return Container.Resolve<T>();
      }

      public static T Resolve<T>(string componentKey)
      {
         return Container.Resolve<T>(componentKey);
      }

      private static void CreateContainer()
      {
          if (_container != null) return;
          _container = new WindsorContainer();
          NuBootstrapper.Configure(_container);
      }

       public static void InitializeContainer(IWindsorContainer container)
      {
         _container = container;
      }
   }
}