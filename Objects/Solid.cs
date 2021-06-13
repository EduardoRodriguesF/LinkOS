using Microsoft.Xna.Framework;
using Pirita.Objects;
using Pirita.Pools;

namespace LinkOS.Objects {
    public class Solid : GameObject, IPoolable {
        public bool PoolIsValid { get; set; }
        public bool PoolIsFree { get; set; }

        public Solid() {
            Scale = new Vector2(2);
            AddHitbox(new Pirita.Collision.Hitbox(Vector2.Zero, 8, 8));
        }

        public virtual void Initialize() {
            Destroyed = false;
        }

        public void Release() {
            Destroy();
        }
    }
}
