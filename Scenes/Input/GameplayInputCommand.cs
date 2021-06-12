using Pirita.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Scenes.Input {
    public class GameplayInputCommand : InputCommand {
        public class Left : InputCommand { }
        public class Right : InputCommand { }
        public class Up : InputCommand { }
        public class Down : InputCommand { }
        public class Restart : InputCommand { }
    }
}
