using Pirita.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Scenes {
    public class GameplayEvent : Event {
        public class RobotsMoved : GameplayEvent { }
        public class DoorsToggle : GameplayEvent { }
    }
}
