using StructureMap;

namespace PaulBenchmark
{
	public class StructureMap : IPaulTest
	{
		private readonly Container container;

		public StructureMap()
		{
			container = new Container(e=>
			                          	{
			                          		e.ForSingletonOf<Game>().Use<Game>();
											e.For<Player>().Use<Player>();
											e.For<Gun>().Use<Gun>();
											e.For<Bullet>().Use<Bullet>();
			                          	});
		}

		#region IPaulTest Members

		public Player ResolvePlayer()
		{
			return container.GetInstance<Player>();
		}

		#endregion
	}
}