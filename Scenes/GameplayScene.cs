using LinkOS.Objects;
using Microsoft.Xna.Framework;
using Pirita.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using static Pirita.Pools.IPoolable;

namespace LinkOS.Scenes {
    public class GameplayScene : Scene {
        private const int MaxEnergy = 100;
        private int _energy;

        private Pool<ChamberItem> _chamberItemPool;
        private List<ChamberItem> _chamberItemList;

        public override void LoadContent() {
            GenerateLevel();
        }

        private void GenerateLevel() {
            _chamberItemPool = new Pool<ChamberItem>(12);
            _chamberItemList = new List<ChamberItem>();
        }

        public override void UpdateGameState(GameTime gameTime) {
            UpdateObjects(gameTime);

            PostUpdateObjects(gameTime);
        }

        protected override void SetInputManager() {
            InputManager = new Pirita.Input.InputManager(new GameplayInputMapper());
        }
    }
}
