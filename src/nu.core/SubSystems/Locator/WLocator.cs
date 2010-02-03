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
namespace nu.core.SubSystems.Locator
{
	using Castle.Windsor;

	public static class WLocator
	{
		static IWindsorContainer _container;

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

		public static void InitializeContainer(IWindsorContainer container)
		{
			_container = container;
		}

		static void CreateContainer()
		{
			if (_container != null)
				return;
			_container = new WindsorContainer();
			NuBootstrapper.Configure(_container);
		}
	}
}