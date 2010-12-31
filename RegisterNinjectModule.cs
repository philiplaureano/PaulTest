using System;
using Ninject;
using Ninject.Modules;

namespace PaulBenchmark
{
	public class RegisterNinjectModule:NinjectModule
	{
		public override void Load()
		{
			this.Bind<Game>().To<Game>().InSingletonScope();
			this.Bind<Player>().To<Player>().InTransientScope();
			this.Bind<Gun>().To<Gun>().InTransientScope();
			this.Bind<Bullet>().To<Bullet>().InTransientScope();
			this.Bind<Func<Bullet>>().ToMethod(c => () => c.Kernel.TryGet<Bullet>());
		}
	}
}