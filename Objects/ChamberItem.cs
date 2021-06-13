using Microsoft.Xna.Framework;
using Pirita.Pools;

namespace LinkOS.Objects {
    public class ChamberItem : Solid, IPoolable {
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
        public ItemType Type { get; set; }
        public enum ItemType : int {
            Door = 0,
            Exit = 1,
            Button = 2,
        }

        public ChamberItem() {
            Initialize();
            Scale = new Vector2(2);
            AddHitbox(new Pirita.Collision.Hitbox(Vector2.Zero, 8, 8));
        }

        public override void Initialize() {
            base.Initialize();
            IsActive = false;
            IsOnline = true;
            Destroyed = false;
        }

        public void ToggleActive() {
            SetActive(!IsActive);
        }

        public void SetActive(bool active) {
            IsActive = active;
            if (Type == ItemType.Door) Opacity = !active ? 1f : 0f;
        }

        public void ToggleConnection() {
            IsOnline = !IsOnline;
        }
    }
}
