using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace PaulBenchmark
{
	public class Windsor_delegates : IPaulTest
	{
		private readonly IWindsorContainer container;

		public Windsor_delegates()
		{
			container = new WindsorContainer()
				.AddFacility<TypedFactoryFacility>()
				.Register(Component.For<Game>().UsingFactoryMethod(() => new Game()),
				          Component.For<Player>().UsingFactoryMethod(k => new Player(k.Resolve<Game>(), k.Resolve<Gun>())).LifeStyle.Transient,
				          Component.For<Gun>().UsingFactoryMethod(k => new Gun(k.Resolve<Game>(), k.Resolve<Func<Bullet>>())).LifeStyle.Transient,
				          Component.For<Bullet>().UsingFactoryMethod(k => new Bullet(k.Resolve<Game>())).LifeStyle.Transient,
				          Component.For<Func<Bullet>>().AsFactory());
		}

		#region IPaulTest Members

		public Player ResolvePlayer()
		{
			return container.Resolve<Player>();
		}

		#endregion
	}
}