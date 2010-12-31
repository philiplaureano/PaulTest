using Autofac;

namespace PaulBenchmark
{
	public class Autofac : IPaulTest
	{
		private readonly IContainer container;

		public Autofac()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<Game>().SingleInstance();
			builder.RegisterType<Player>().InstancePerDependency();
			builder.RegisterType<Gun>().InstancePerDependency();
			builder.RegisterType<Bullet>().InstancePerDependency();

			container = builder.Build();
		}

		#region IPaulTest Members

		public Player ResolvePlayer()
		{
			return container.Resolve<Player>();
		}

		#endregion
	}
}