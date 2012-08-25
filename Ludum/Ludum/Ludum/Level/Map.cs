using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Ludum.Level;

namespace Ludum.Level
{
    class Map : GameScreen
    {
        private Foregound foreground;
        public int Width;
        public int Height;
        public const int TILE_SIZE = 32;
        public Tile[,] TileMap;

        public Map(String name, GameScreen parent) : base(name, 0, parent)
        {

        }


        
    }
}
