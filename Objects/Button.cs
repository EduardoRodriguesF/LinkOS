using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirita.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LinkOS.Objects {
    public class Button : Drawable {
        private Rectangle _bounds;

        public Texture2D Texture;

        public override int Width => _bounds.Width;
        public override int Height => _bounds.Height;

        public Rectangle Bounds {
            get => new Rectangle((int)(Position.X), (int)(Position.Y), (int)(Width*Scale.X), (int)(Height*Scale.Y));
            private set => _bounds = value; 
        }

        public Button(int x, int y, int width, int height) {
            Position = new Vector2(x, y) * 2;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        public override void Render(SpriteBatch spriteBatch) {
            if (Texture != null)
                spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Width, Height), Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}
