using Microsoft.Xna.Framework;
using Pirita.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Objects {
    public class Robot : GameObject {
        private const float Speed = 1f;

        public Vector2 Velocity;

        public void MoveLeft() {
            Stop();
            Velocity.X = -Speed;
        }

        public void MoveRight() {
            Stop();
            Velocity.X = Speed;
        }
        
        public void MoveUp() {
            Stop();
            Velocity.Y = -Speed;
        }
        
        public void MoveDown() {
            Stop();
            Velocity.Y = Speed;
        }

        public void Stop() {
            Velocity = Vector2.Zero;
        }

        public override void PostUpdate(GameTime gameTime) {
            Position += Velocity;
        }
    }
}
