using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using CarmineEngine.Level;
using Microsoft.Xna.Framework;

namespace Ludum.Level
{
    class GameState : GameScreen
    {
        public static int TILE_SIZE = 64;

        public Dictionary<String, Level> Levels = new Dictionary<string,Level>();
        public Level CurrentLevel;
        public Player Player;

        public GameState()
            : base("levelState")
        {
        }

        public override void update()
        {
            base.update();
        }

        public void loadLevels()
        {
            this.LoadFolder("levels");
        }

        internal void LoadFolder(string folderDir)
        {
            Dictionary<string, LevelData> templates = Engine.LoadFolder<LevelData>(folderDir);
            foreach (KeyValuePair<string, LevelData> pair in templates)
                ProcessData(pair.Value);
        }

        internal void ProcessData(LevelData levelData)
        {
            Level level = new Level(levelData.name, this);
            Levels.Add(levelData.name, level);
            level.Map = MapLoader.loadRawMap(Engine.Content.Load<MapData>("levels/maps/" + levelData.map), level);
            if (levelData.start)
            {
                SpawnPoint spawn = new SpawnPoint();
                spawn.entryPoint = new Point(1, 1);
                level.Map.SpawnPoints.TryGetValue("start", out spawn);
                SwitchToLevel(levelData.name, spawn.entryPoint);
            }
            else
            {
                level.AcceptUpdate = false;
                level.AcceptDraw = false;
            }
        }

        public void SwitchToLevel(String level, Point entryPoint)
        {
            if (CurrentLevel != null)
            {
                CurrentLevel.AcceptUpdate = false;
                CurrentLevel.AcceptDraw = false;
            }
            Levels.TryGetValue(level, out CurrentLevel);
            Player.moveTo(entryPoint.X * TILE_SIZE, entryPoint.Y * TILE_SIZE);
            CurrentLevel.AcceptUpdate = true;
            CurrentLevel.AcceptDraw = true;
        }
    }
}
