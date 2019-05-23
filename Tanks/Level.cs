using System.Collections.Generic;

namespace Tanks
{
    public class Level
    {
        public List<Actor> Actors { get; }
        public List<Projectile> Projectiles { get; }
        public Actor Player { get; }

        public Level()
        {
            Projectiles = new List<Projectile>();
            Player = new Actor();
            Actors = new List<Actor> { Player };
        }
    }
}
