using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tanks
{
    public class Game
    {
        public Level CurrentLevel { get; set; }

        public Random Random { get; }

        private RectangleF BoundingRectangle { get; }

        public Game(int w, int h)
        {
            BoundingRectangle = new RectangleF(0, 0, w, h);
            CurrentLevel = new Level();
            Random = new Random();
            PendingActions = new HashSet<InputAction>();
            InputHandlers = new Dictionary<InputAction, Action>
            {
                [InputAction.RotateLeft] = RotateLeft,
                [InputAction.RotateRight] = RotateRight,
                [InputAction.MoveForward] = MoveForward,
                [InputAction.MoveBackward] = MoveBackward,
                [InputAction.Shoot] = Shoot
            };
            CurrentLevel.Player.X = 300;
            CurrentLevel.Player.Y = 300;
        }

        private void MoveForward()
        {
            CurrentLevel.Player.X += (float)Math.Cos(CurrentLevel.Player.Rotation) * MovementVelocity;
            CurrentLevel.Player.Y += (float)Math.Sin(CurrentLevel.Player.Rotation) * MovementVelocity;
        }

        private void MoveBackward()
        {
            CurrentLevel.Player.X -= (float)Math.Cos(CurrentLevel.Player.Rotation) * MovementVelocity;
            CurrentLevel.Player.Y -= (float)Math.Sin(CurrentLevel.Player.Rotation) * MovementVelocity;
        }

        private void Shoot()
        {
            CurrentLevel.Projectiles.Add(new Projectile { X = 300, Y = 300, Angle = CurrentLevel.Player.Rotation, Owner = CurrentLevel.Player, Velocity = 10 });
        }

        public HashSet<InputAction> PendingActions { get; }

        private Dictionary<InputAction, Action> InputHandlers { get; }

        private const float RotationDelta = 0.1f;
        private const float PI2 = 6.283185F;
        private const float MovementVelocity = 3;

        private void RotateLeft()
        {
            CurrentLevel.Player.Rotation -= RotationDelta;
            if (CurrentLevel.Player.Rotation < 0)
            {
                CurrentLevel.Player.Rotation = PI2;
            }
        }

        private void RotateRight()
        {
            CurrentLevel.Player.Rotation += RotationDelta;
            if (CurrentLevel.Player.Rotation > PI2)
            {
                CurrentLevel.Player.Rotation = 0;
            }
        }

        public void Update()
        {
            foreach (var action in PendingActions)
            {
                InputHandlers[action].Invoke();
            }
            PendingActions.Clear();

            CurrentLevel.Projectiles.ForEach(p => p.Update());
            CurrentLevel.Projectiles.RemoveAll(p => !BoundingRectangle.Contains(p.X, p.Y));
        }
    }
}
