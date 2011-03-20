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
			// no way to map func

			container = map.CreateContainer();
		}


		public Player ResolvePlayer()
		{
			throw new NotSupportedException("No way to have the Func<Bullet> provided");
			//return container.GetInstance<Player>();
		}
	}

}