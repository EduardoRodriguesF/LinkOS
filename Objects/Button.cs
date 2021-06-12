using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirita.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkOS.Objects {
    public class Button : Drawable {
        private Rectangle _bounds;

        public Texture2D Texture;

        public override int Width => _bounds.Width;
        public override int Height => _bounds.Height;

        public Rectangle Bounds {
            get => new Rectangle((int)(Position.X + InitialPosition.X), (int)(Position.Y + InitialPosition.Y), Width, Height);
            private set => _bounds = value; 
        }

        public Button(int x, int y, int width, int height) {
            Position = new Vector2(x, y);
            Bounds = new Rectangle(x, y, width, height);
        }

        public override void Render(SpriteBatch spriteBatch) {
            if (Texture != null)
                spriteBatch.Draw(Texture, Bounds, new Rectangle(0, 0, Width, Height), Color.White, Rotation, Origin, SpriteEffects.None, 1);
        }
    }
}
