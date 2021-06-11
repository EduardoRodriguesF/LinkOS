using Pirita.Objects;
using Pirita.Pools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Objects {
    public class ChamberItem : GameObject, IPoolable {
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }

        public bool PoolIsValid { get; set; }
        public bool PoolIsFree { get; set; }

        public ChamberItem() {
            Initialize();
        }

        public void Initialize() {
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

        public void Release() { }
    }
}
