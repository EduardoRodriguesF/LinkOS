using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirita;

namespace LinkOS.Objects {
    public class EnergyBar : HudElement {
        private const int MaxEnergy = 76;

        public EnergyBar(float x, float y, Texture2D texture) : base(x, y, texture) {
            Position = new Vector2(x, y);
            Texture = texture;
        }

        public void Update(int energy) {
            InitialScale = new Vector2(Utils.Lerp(InitialScale.X, (energy / 100f) * 76, 0.25f), 1);
        }
    }
}
