namespace PaulBenchmark
{
	public class CustomContainer : IPaulTest
	{
		private readonly Game game = new Game();

		#region IPaulTest Members

		public Player ResolvePlayer()
		{
			return new Player(game, ResolveGun());
		}

		#endregion

		public Game ResolveGame()
		{
			return game;
		}

		public Gun ResolveGun()
		{
			return new Gun(game, ResolveBullet);
		}

		public Bullet ResolveBullet()
		{
			return new Bullet(game);
		}
	}
}