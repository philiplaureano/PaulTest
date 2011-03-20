using System;
using Hiro;
using Hiro.Containers;

namespace PaulBenchmark
{
	public class Hiro: IPaulTest
	{
		private readonly IMicroContainer container;

		public Hiro()
		{
			var map = new DependencyMap();
			map.AddSingletonService<Game, Game>();
			map.AddService<Player, Player>();
			map.AddService<Gun, Gun>();
			map.AddService<Bullet, Bullet>();
			map.AddService<Func<Bullet>>(k => () => k.GetInstance<Bullet>());
			container = map.CreateContainer();
		}


		public Player ResolvePlayer()
		{
			return container.GetInstance<Player>();
		}
	}

}