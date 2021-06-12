using Microsoft.Xna.Framework;
using Pirita.Objects;
using Pirita.Pools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Objects {
    public class ChamberItem : Solid, IPoolable {
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
        public ItemType Type { get; set; }
        public enum ItemType : int {
            Door = 0,
        }

        public ChamberItem() {
            Initialize();
            AddHitbox(new Pirita.Collision.Hitbox(Vector2.Zero, 16, 16));
        }

        public override void Initialize() {
            base.Initialize();
            IsActive = false;
            IsOnline = true;
            Destroyed = false;
        }

        public void ToggleActive() {
            IsActive = !IsActive;
        }

        public void ToggleConnection() {
            IsOnline = !IsOnline;
        }
    }
}
