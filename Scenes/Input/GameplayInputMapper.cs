using LinkOS.Scenes.Input;
using Microsoft.Xna.Framework.Input;
using Pirita.Input;
using System.Collections.Generic;

namespace LinkOS.Scenes {
    public class GameplayInputMapper : InputMapper {
        public override IEnumerable<InputCommand> GetKeyboardState(KeyboardState state, KeyboardState oldState) {
            var commands = (List<InputCommand>)base.GetKeyboardState(state, oldState);

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

            if (Pressed(Keys.H, state, oldState))
                commands.Add(new GameplayInputCommand.ToggleDoors());

            return commands;
        }

        public override IEnumerable<InputCommand> GetMouseState(MouseState state, MouseState oldState) {
            var commands = new List<InputCommand>();

            if (Pressed(state.LeftButton, oldState.LeftButton)) {
                commands.Add(new GameplayInputCommand.Click());
            }

            return commands;
        }
    }
}
