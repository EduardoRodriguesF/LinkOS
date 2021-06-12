using Microsoft.Xna.Framework;
using Pirita.Objects;
using Pirita.Pools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Objects {
    public class Solid : GameObject, IPoolable {
        public bool PoolIsValid { get; set; }
        public bool PoolIsFree { get; set; }

        public Solid() {
            AddHitbox(new Pirita.Collision.Hitbox(Vector2.Zero, 16, 16));
        }

        public virtual void Initialize() {
            Destroyed = false;
        }

        public void Release() {
            Destroy();
        }
    }
}
