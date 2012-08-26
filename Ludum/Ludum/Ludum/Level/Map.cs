using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using CarmineEngine.Level;
using System.Diagnostics;

namespace Ludum.Level
{
    class Map : GameScreen
    {
        private Foregound foreground;
        public int Width;
        public int Height;
        public Tile[,] TileMap;

        public Dictionary<String, SpawnPoint> SpawnPoints;
        public List<LevelDoorData> LevelDoors;

        public Map(String name, GameScreen parent) : base(name, 0, parent)
        {

        }
    }
}
