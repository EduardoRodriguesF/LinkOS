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

        private Button _btnMoveLeft;
        private Button _btnMoveRight;
        private Button _btnMoveUp;
        private Button _btnMoveDown;
        private Button _btnStop;
        private Button _btnDoors;

        private Texture2D _doorTexture;
        private Texture2D _solidTexture;
        private Texture2D _robotTexture;

        private Vector2 _mousePos;

        public List<ChamberItem> Doors => _chamberItemList.FindAll(c => c.Type == ItemType.Door);

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
            _doorTexture = LoadTexture("Sprites/Map/door");
            _robotTexture = LoadTexture("Sprites/Map/robot");
            _solidTexture = LoadTexture("Sprites/Map/solid");

            _robotList = new List<Robot>();
            _chamberItemList = new List<ChamberItem>();
            _solidList = new List<Solid>();

            _robotPool = new Pool<Robot>(2);
            _chamberItemPool = new Pool<ChamberItem>(12);
            _solidPool = new Pool<Solid>(24);

            

            LoadHud();

            GenerateLevel();
        }

        private void LoadHud() {
            Hud.AddElement(new HudElement(0, 0, LoadTexture("Sprites/UI/staticHud")));

            _btnMoveLeft = new Button(184, 48, 16, 16) { Texture = LoadTexture("Sprites/UI/Buttons/left") };
            _btnMoveRight = new Button(220, 48, 16, 16) { Texture = LoadTexture("Sprites/UI/Buttons/right") };
            _btnMoveUp = new Button(202, 30, 16, 16) { Texture = LoadTexture("Sprites/UI/Buttons/up") };
            _btnMoveDown = new Button(202, 66, 16, 16) { Texture = LoadTexture("Sprites/UI/Buttons/down") };
            _btnStop = new Button(202, 48, 16, 16) { Texture = LoadTexture("Sprites/UI/Buttons/stop") };
            _btnDoors = new Button(172, 90, 38, 17) { Texture = LoadTexture("Sprites/UI/Buttons/doors") };
            Hud.AddElement(_btnMoveLeft);
            Hud.AddElement(_btnMoveRight);
            Hud.AddElement(_btnMoveUp);
            Hud.AddElement(_btnMoveDown);
            Hud.AddElement(_btnStop);
            Hud.AddElement(_btnDoors);

            Debug.WriteLine(_btnMoveUp.Bounds);

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

        private void StopRobots() {
            _robotList.ForEach(r => r.Stop());
        }

        private void ToggleDoors() {
            Doors.ForEach(d => d.IsActive = !d.IsActive);
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

            var m = InputManager.GetMousePosition();
            _mousePos.X = (int)((m.X + Camera.Position.X - (Viewport.Width / 2)) / Camera.Zoom);
            _mousePos.Y = (int)((m.Y + Camera.Position.Y - (Viewport.Height / 2)) / Camera.Zoom);

            InputManager.GetCommands(cmd => {
                if (cmd is GameplayInputCommand.Click) {
                    var mRect = new Rectangle((int)_mousePos.X, (int)_mousePos.Y, 1, 1);
                    if (mRect.Intersects(_btnMoveLeft.Bounds)) {
                        TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Left));
                    } else if (mRect.Intersects(_btnMoveRight.Bounds)) {
                        TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Right));
                    } else if (mRect.Intersects(_btnMoveUp.Bounds)) {
                        TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Up));
                    } else if (mRect.Intersects(_btnMoveDown.Bounds)) {
                        TryAction(EnergyCost.RobotMovement, () => MoveRobots(Direction.Down));
                    } else if (mRect.Intersects(_btnStop.Bounds)) {
                        TryAction(EnergyCost.RobotMovement, () => StopRobots());
                    } else if (mRect.Intersects(_btnDoors.Bounds)) {
                        TryAction(EnergyCost.DoorToggle, () => ToggleDoors());
                    }

                    Debug.WriteLine(mRect);
                }

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
                
                if (cmd is GameplayInputCommand.ToggleDoors) {
                    TryAction(EnergyCost.DoorToggle, () => ToggleDoors());
                }
            });
        }

        public override void UpdateGameState(GameTime gameTime) {
            UpdateObjects(gameTime);

            CheckCollisions();
            Hud.Update(Camera.Position, Camera.Zoom);

            PostUpdateObjects(gameTime);

            CleanLists();
        }

        private void CheckCollisions() {
            var filteredList = new List<Solid>();
            _solidList.ForEach(d => {
                filteredList.Add(d);
            });
            Doors.ForEach(d => {
                if (d.IsActive) {
                    if (filteredList.Contains(d)) {
                        filteredList.Remove(d);
                    }
                }
            });
            var robotsCD = new AABBCollisionDetector<Solid, Robot>(filteredList);

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

        private void SpawnDoor(float x, float y, bool active = false) {
            var door = _chamberItemPool.Get();
            door.Type = ChamberItem.ItemType.Door;
            door.Position = new Vector2(x, y);
            door.IsActive = active;
            door.SetTexture(_doorTexture);
            _chamberItemList.Add(door);
            _solidList.Add(door);
            AddObject(door);
        }

        private void SpawnRobot(float x, float y) {
            var robot = _robotPool.Get();
            robot.Position = new Vector2(x, y);
            robot.SetTexture(_robotTexture);
            _robotList.Add(robot);
            AddObject(robot);
        }

        private void SpawnSolid(float x, float y) {
            var solid = _solidPool.Get();
            solid.Position = new Vector2(x, y);
            solid.SetTexture(_solidTexture);
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

        protected override void SetCamera() {
            base.SetCamera();
        }

        private void CleanLists() {
            _chamberItemList = CleanObjects(_chamberItemList);
            _robotList = CleanObjects(_robotList);
            _solidList = CleanObjects(_solidList);
        }
    }
}
