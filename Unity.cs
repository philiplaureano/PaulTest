using System;
using Microsoft.Practices.Unity;

namespace PaulBenchmark
{
	public class Unity : IPaulTest
	{
		private readonly UnityContainer container;

		public Unity()
		{
			container = new UnityContainer();
			container.RegisterType<Game>(new ContainerControlledLifetimeManager());
			container.RegisterType<Player>(new TransientLifetimeManager());
			container.RegisterType<Gun>(new TransientLifetimeManager());
			container.RegisterType<Bullet>(new TransientLifetimeManager());
			container.RegisterInstance(new Func<Bullet>(() => container.Resolve<Bullet>()));

		}

		#region IPaulTest Members

		public Player ResolvePlayer()
		{
			return container.Resolve<Player>();
		}

		#endregion
	}
}