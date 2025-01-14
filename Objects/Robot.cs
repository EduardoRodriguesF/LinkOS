﻿using Microsoft.Xna.Framework;
using Pirita.Objects;
using Pirita.Pools;

namespace LinkOS.Objects {
    public class Robot : GameObject, IPoolable {
        private const float Speed = 10f;

        public Vector2 Velocity;

        public bool PoolIsValid { get; set; }
        public bool PoolIsFree { get; set; }

        public Robot() {
            Scale = new Vector2(2);
            AddHitbox(new Pirita.Collision.Hitbox(Vector2.Zero, 8, 8));
        }

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

        public void Initialize() {
            Velocity = Vector2.Zero;
            Destroyed = false;
        }

        public void Release() {
            Destroy();
        }
    }
}
