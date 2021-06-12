using LinkOS.Scenes.Input;
using Microsoft.Xna.Framework.Input;
using Pirita.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Scenes {
    public class GameplayInputMapper : InputMapper {
        public override IEnumerable<InputCommand> GetKeyboardState(KeyboardState state, KeyboardState oldState) {
            var commands = (List<InputCommand>) base.GetKeyboardState(state, oldState);

            if (Pressed(Keys.A, state, oldState))
                commands.Add(new GameplayInputCommand.Left());

            if (Pressed(Keys.D, state, oldState))
                commands.Add(new GameplayInputCommand.Right());

            if (Pressed(Keys.W, state, oldState))
                commands.Add(new GameplayInputCommand.Up());

            if (Pressed(Keys.S, state, oldState))
                commands.Add(new GameplayInputCommand.Down());

            if (Pressed(Keys.R, state, oldState))
                commands.Add(new GameplayInputCommand.Restart());

            return commands;
        }
    }
}
