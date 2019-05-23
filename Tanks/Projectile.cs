using System;

namespace Tanks
{
    public class Projectile : IBehaviour
    {
        public Actor Owner { get; set; }
        public float Velocity { get; set; }
        public float Angle { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        
        public void Update()
        {
            X += (float)Math.Cos(Angle) * Velocity;
            Y += (float)Math.Sin(Angle) * Velocity;
        }
    }
}
