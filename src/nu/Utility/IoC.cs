namespace nu
{
   using System.Configuration;
   using Castle.Windsor;

   public static class IoC
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
          string windsorConfig = ConfigurationManager.AppSettings["windsor"];
          _container = new WindsorContainer(windsorConfig);
          //NuConfiguration.Configure(_container);
      }

       public static void InitializeContainer(IWindsorContainer container)
      {
         _container = container;
      }
   }
}