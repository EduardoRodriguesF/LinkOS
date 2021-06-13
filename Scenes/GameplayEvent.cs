using Pirita.Scenes;

namespace LinkOS.Scenes {
    public class GameplayEvent : Event {
        public class RobotsMoved : GameplayEvent { }
        public class DoorsToggle : GameplayEvent { }
    }
}
