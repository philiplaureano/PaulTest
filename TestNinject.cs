using Ninject;

namespace PaulBenchmark
{
	public class Ninject:IPaulTest
	{
		private readonly StandardKernel kernel;

		public Ninject()
		{
			kernel = new StandardKernel(new RegisterNinjectModule());
		}

		public Player ResolvePlayer()
		{
			return kernel.TryGet<Player>();
		}
	}
}