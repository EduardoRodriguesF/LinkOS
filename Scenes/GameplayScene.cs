using LinkOS.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirita.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using static LinkOS.Objects.ChamberItem;
using static Pirita.Pools.IPoolable;

namespace LinkOS.Scenes {
    public class GameplayScene : Scene {
        private const int MaxEnergy = 100;
        private int _energy;

        private Pool<ChamberItem> _chamberItemPool;
        private List<ChamberItem> _chamberItemList;

        private Texture2D _doorTexture;

        public override void LoadContent() {
            _doorTexture = LoadTexture("placeholder");

            _chamberItemPool = new Pool<ChamberItem>(12);
            _chamberItemList = new List<ChamberItem>();

            GenerateLevel();
        }

        private void GenerateLevel() {
            foreach (var item in _chamberItemList) {
                _chamberItemPool.Release(item);
            }

            SpawnDoor(0, 0);
        }

        public override void UpdateGameState(GameTime gameTime) {
            UpdateObjects(gameTime);

            PostUpdateObjects(gameTime);

            _chamberItemList = CleanObjects(_chamberItemList);
        }

        private void SpawnDoor(float x, float y) {
            var door = _chamberItemPool.Get();
            door.Position = new Vector2(x, y);
            door.SetTexture(_doorTexture);
            door.AddHitbox(new Pirita.Collision.Hitbox(Vector2.Zero, 16, 16));
            _chamberItemList.Add(door);
            AddObject(door);
        }

        protected override void SetInputManager() {
            InputManager = new Pirita.Input.InputManager(new GameplayInputMapper());
        }
    }
}
