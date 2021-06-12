using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirita.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Objects {
    public class HudElement : Drawable {
        public Texture2D Texture;

        public override int Width => Texture.Width;
        public override int Height => Texture.Height;

        public HudElement(float x, float y, Texture2D texture) {
            Position = new Vector2(x, y);
            Texture = texture;
        }

        public override void Render(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Width, Height), Color.White * Opacity, Rotation, Origin, new Vector2(Math.Abs(Scale.X), Math.Abs(Scale.Y)), SpriteEffects.None, 0f);
        }
    }
}
