using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace PaulBenchmark
{
	public class Windsor : IPaulTest
	{
		private readonly IWindsorContainer container;

		public Windsor()
		{
			container = new WindsorContainer()
				.AddFacility<TypedFactoryFacility>()
				.Register(Component.For<Game>(),
				          Component.For<Player>().LifeStyle.Transient,
				          Component.For<Gun>().LifeStyle.Transient,
				          Component.For<Bullet>().LifeStyle.Transient,
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