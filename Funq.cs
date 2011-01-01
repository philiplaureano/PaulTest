using System;
using Funq;

namespace PaulBenchmark
{
	public class Funq : IPaulTest
	{
		private readonly Container container;

		public Funq()
		{
			container = new Container();
			container.Register(c => new Game()).ReusedWithin(ReuseScope.Container);
			container.Register(c => new Player(c.Resolve<Game>(), c.Resolve<Gun>())).ReusedWithin(ReuseScope.None);
			container.Register(c => new Gun(c.Resolve<Game>(), c.LazyResolve<Bullet>())).ReusedWithin(ReuseScope.None);
			container.Register(c => new Bullet(c.Resolve<Game>())).ReusedWithin(ReuseScope.None);
		}

		#region IPaulTest Members

		public Player ResolvePlayer()
		{
			return container.Resolve<Player>();
		}

		#endregion
	}
}