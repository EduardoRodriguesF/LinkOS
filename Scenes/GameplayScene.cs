using LinkOS.Objects;
using LinkOS.Scenes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirita.Collision;
using Pirita.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static LinkOS.Objects.ChamberItem;
using static Pirita.Pools.IPoolable;

namespace LinkOS.Scenes {
    public class GameplayScene : Scene {
        private const int MaxEnergy = 100;
        private int _energy;

        private Pool<ChamberItem> _chamberItemPool;
        private Pool<Robot> _robotPool;
        private Pool<Solid> _solidPool;

        private List<ChamberItem> _chamberItemList;
        private List<Robot> _robotList;
        private List<Solid> _solidList;

        private Texture2D _doorTexture;

        private enum Direction {
            Left,
            Up,
            Right,
            Down,
        }

        private enum EnergyCost : int {
            RobotMovement = 5,
            DoorToggle = 10,
        }

        public override void LoadContent() {
            _doorTexture = LoadTexture("placeholder");

            _robotList = new List<Robot>();
            _chamberItemList = new List<ChamberItem>();
            _solidList = new List<Solid>();

            _robotPool = new Pool<Robot>(2);
            _chamberItemPool = new Pool<ChamberItem>(12);
            _solidPool = new Pool<Solid>(24);

            GenerateLevel();
        }

        private void GenerateLevel() {
            _energy = MaxEnergy;

            foreach (var item in _chamberItemList) {
                _chamberItemPool.Release(item);
            }
            
            foreach (var solid in _solidList) {
                if (!(solid is ChamberItem)) _solidPool.Release(solid);
            }

            foreach (var robot in _robotList) {
                _robotPool.Release(robot);
            }

            CleanLists();

            SpawnDoor(0, 0);
            SpawnRobot(-100, 0);
            SpawnSolid(64, 64);
        }

        private void MoveRobots(Direction direction) {
            switch (direction) {
                case Direction.Left:
                    _robotList.ForEach(r => r.MoveLeft());
                    break;
                case Direction.Right:
                    _robotList.ForEach(r => r.MoveRight());
                    break;
                case Direction.Up:
                    _robotList.ForEach(r => r.MoveUp());
                    break;
                case Direction.Down:
                    _robotList.ForEach(r => r.MoveDown());
                    break;
            }
        }

        private void TryAction(EnergyCost energyCost, Action action) {
            TryAction((int)energyCost, action);
        }

        private void TryAction(int energyAmount, Action action) {
            if (_energy - energyAmount < 0) return;

            action();
            ConsumeEnergy(energyAmount);
        }

        public override void HandleInput(GameTime gameTime) {
            base.HandleInput(gameTime);

            InputManager.GetCommands(cmd => {
                if (cmd is GameplayInputCommand.Left) {
                    TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Left));
                } else if (cmd is GameplayInputCommand.Right) {
                    TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Right));
                } else if (cmd is GameplayInputCommand.Up) {
                    TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Up));
                } else if (cmd is GameplayInputCommand.Down) {
                    TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Down));
                }

                if (cmd is GameplayInputCommand.Restart) {
                    GenerateLevel();
                }
            });
        }

        public override void UpdateGameState(GameTime gameTime) {
            UpdateObjects(gameTime);

            CheckCollisions();

            PostUpdateObjects(gameTime);

            CleanLists();
        }

        private void CheckCollisions() {
            var robotsCD = new AABBCollisionDetector<Solid, Robot>(_solidList);

            Vector2 pos;
            int velDir;
            foreach (var robot in _robotList) {
                pos = new Vector2(robot.Velocity.X, 0);

                if (robotsCD.DetectCollisions(robot, pos)) {
                    velDir = Math.Sign(robot.Velocity.X);
                    pos.X = velDir;

                    while (!robotsCD.DetectCollisions(robot, pos)) {
                        robot.Position += new Vector2(velDir, 0);
                    }

                    robot.Velocity.X = 0;
                }

                pos = new Vector2(0, robot.Velocity.Y);

                if (robotsCD.DetectCollisions(robot, pos)) {
                    velDir = Math.Sign(robot.Velocity.Y);
                    pos.Y = velDir;

                    while (!robotsCD.DetectCollisions(robot, pos)) {
                        robot.Position += new Vector2(0, velDir);
                    }

                    robot.Velocity.Y = 0;
                }
            }
        }

        private void SpawnDoor(float x, float y) {
            var door = _chamberItemPool.Get();
            door.Position = new Vector2(x, y);
            door.SetTexture(_doorTexture);
            _chamberItemList.Add(door);
            _solidList.Add(door);
            AddObject(door);
        }

        private void SpawnRobot(float x, float y) {
            var robot = _robotPool.Get();
            robot.Position = new Vector2(x, y);
            robot.SetTexture(_doorTexture);
            _robotList.Add(robot);
            AddObject(robot);
        }

        private void SpawnSolid(float x, float y) {
            var solid = _solidPool.Get();
            solid.Position = new Vector2(x, y);
            solid.SetTexture(_doorTexture);
            _solidList.Add(solid);
            AddObject(solid);
        }

        private void ConsumeEnergy(int amount) {
            _energy -= amount;

            Debug.WriteLine(_energy);
        }

        protected override void SetInputManager() {
            InputManager = new Pirita.Input.InputManager(new GameplayInputMapper());
        }

        private void CleanLists() {
            _chamberItemList = CleanObjects(_chamberItemList);
            _robotList = CleanObjects(_robotList);
            _solidList = CleanObjects(_solidList);
        }
    }
}
