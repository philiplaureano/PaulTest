using System;
using Hiro;
using Hiro.Containers;

namespace PaulBenchmark
{
    public class Hiro : IPaulTest
    {
        private readonly IMicroContainer container;

        public Hiro()
        {
            var map = new DependencyMap();
            map.AddSingletonService<Game, Game>();
            map.AddService<Player, Player>();
            map.AddService<Gun, Gun>();
            map.AddService<Bullet, Bullet>();

            Func<IMicroContainer, Bullet> createBullet = c => c.GetInstance<Bullet>();
            Func<IMicroContainer, Func<Bullet>> createBulletFunctor = c => () => createBullet(c);
            map.AddService(createBulletFunctor);

            container = map.CreateContainer();
        }


        public Player ResolvePlayer()
        {
            return container.GetInstance<Player>();
        }
    }
}